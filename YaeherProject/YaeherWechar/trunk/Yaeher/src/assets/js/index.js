import axios from 'axios'
import md5 from 'js-md5'
import { createSecret } from 'assets/js/common.js'
import { Toast } from 'mint-ui';
import router from '@/router/index.js'


// 测试
// let url5000 = 'http://192.168.2.3:5000/'
// let url5001 = 'http://192.168.2.3:5001/'
// let url5002 = 'http://192.168.2.3:5002/'

// 线上
// let url5000 = 'http://patient.integraltel.com/'
// let url5001 = 'http://doctor.integraltel.com/'
// let url5002 = 'http://admin.integraltel.com/'

// 健康
let url5000 = 'http://patient.yaeherhealth.com/'
let url5001 = 'http://doctor.yaeherhealth.com/'
let url5002 = 'http://admin.yaeherhealth.com/'

// let userName = ['liuweiwei', 'guanyu',  'qianyue', 'huxiaowen', 'admin', 'gelin', 'zhangsan']
// const userNameOrEmailAddress = userName[1]

// const password = md5('123456')
let secret = createSecret()
// axios.defaults.headers.common[`Authorization`] = userToken
// 登陆获取用户token
const instance = axios.create({
    timeout: 30000,
    headers: {
      'Content-Type': "application/json;charset=utf-8"
    }
});
let tokenFlag = true
let loginFlag = true
var wecharOpenID = ''
// window.localStorage.setItem('compressHead', '?imageView2/q/20')
// window.localStorage.setItem('wecharOpenID1', 'oOiwG1qmIfhnXwJVkgY8ns7aI-g0')
function userLogin (wecharOpenID1) { // 用户登陆
    if (!tokenFlag) return
    tokenFlag = false
    // 登陆
    wecharOpenID = wecharOpenID1
    const url = `${url5002}api/TokenAuth/Authenticate`
    axios.post(url, {
        wecharOpenID,
        platform: 'Mobile',
        secret
    })
        .then((response) => {
            console.log(response.data.result.code)
            let code = response.data.result.code
            // if (response.data.result.code)
            if (code === 200) {
                let userDate = response.data.result.item
                window.sessionStorage.setItem('userToken', userDate.accessToken)
                window.sessionStorage.setItem('userId', userDate.userId)
                window.sessionStorage.setItem('mobileRoleName', userDate.mobileRoleName)
                tokenFlag = true
                loginFlag = true
                window.location.reload()
                let href = location.href;
                let hasQuery = href.indexOf('?') > 0;
                href = href 
                    + (hasQuery ? '&' : '?') 
                    + 'time=' + (new Date().getTime());
                window.location.href = href;
            } else if (code === 402) {
                router.replace({
                    path: '/share-code'
                })
                // 怡芽开发
                // window.location.href = 'https://mp.weixin.qq.com/mp/profile_ext?action=home&__biz=MzU5MDcyODc2Mg==&scene=124#wechat_redirect'

                // 怡芽科技
                // window.location.href = 'https://mp.weixin.qq.com/mp/profile_ext?action=home&__biz=MzU5NDcxNzE1Mw==&scene=124#wechat_redirect'

                // 怡禾健康
                // window.location.href = 'https://mp.weixin.qq.com/mp/profile_ext?action=home&__biz=MzIwMzM3NjI5OQ==&scene=124#wechat_redirect'
            } else {
                Toast(response.data.result.msg)
            }
            

            // return
            // switch (mobileRoleName) { // 返回 401 重新登陆
            //     case 'patient': 
            //         console.log('患者登陆')
            //         router.replace({
            //             path: '/',
            //             query: {redirect: router.currentRoute.fullPath}
            //         })
            //         break;
            //     case 'doctor': 
            //         console.log('医生登陆')
            //         router.replace({
            //             path: '/index-doctor',
            //             query: {redirect: router.currentRoute.fullPath}
            //         })
            //         break;
            //     case 'quality': 
            //         console.log('质控登陆')
            //         router.replace({
            //             path: '/index-control',
            //             query: {redirect: router.currentRoute.fullPath}
            //         })
            //         break;    
            //     case 'customerservice': 
            //         console.log('客服登陆')
            //         router.replace({
            //             path: '/index-customer',
            //             query: {redirect: router.currentRoute.fullPath}
            //         })
            //         break; 
            //     case 'admin': 
            //         console.log('管理登陆')
            //         router.replace({
            //             path: '/index-admin',
            //             query: {redirect: router.currentRoute.fullPath}
            //         })
            //         break;            
            // }
            // setTimeout(function() {
            //     location.reload() 
            // }, 100)
            
        })
        .catch((error) => {
            console.log('获取token失败')
        })
}

// let userToken = window.sessionStorage.getItem(`userToken`)
// let bearer = 'Bearer '
// if (userToken) {
    
// }

instance.interceptors.request.use(
    config => { // 判断token是否存在，存在给每个http请求头加上token
    config.headers['If-Modified-Since'] = '0'
    let userToken = window.sessionStorage.getItem(`userToken`)
    let bearer = 'Bearer '
    config.data.secret = secret
    if (config.data.platform != 'PC') config.data.platform = 'Mobile'
    
    if (userToken) { 
        userToken = bearer + userToken
        config.headers.Authorization = userToken
    }
    return config
  },
  err => {
    return Promise.reject(err)
  })
  
instance.interceptors.response.use(
    response => {
        if (response.data.result.code != 402) {
            if (response.data.result.code != 200 && response.data.result.code != 204 ) {
                Toast(response.data.result.msg)
            } else {
                return response
            }
        } else { // 判断用户是否关注，未关注去公众号关注页面
            router.replace({
                path: '/share-code'
            })
            // 怡芽开发
            // window.location.href = 'https://mp.weixin.qq.com/mp/profile_ext?action=home&__biz=MzU5MDcyODc2Mg==&scene=124#wechat_redirect'

            // 怡芽科技
            // window.location.href = 'https://mp.weixin.qq.com/mp/profile_ext?action=home&__biz=MzU5NDcxNzE1Mw==&scene=124#wechat_redirect'

            // 怡禾健康
            // window.location.href = 'https://mp.weixin.qq.com/mp/profile_ext?action=home&__biz=MzIwMzM3NjI5OQ==&scene=124#wechat_redirect'
        }
        
    },
    error => {
        if (error.response) {
            switch (error.response.status) { // 返回 401 重新登陆
                case 401: console.log(`登陆失效`)
                    if (!loginFlag) {
                        return
                    }
                    loginFlag = false
                    wecharOpenID = window.localStorage.getItem('wecharOpenID1')
                    console.log('indexWecharOpenID', wecharOpenID)
                    if(!wecharOpenID || wecharOpenID === 'undefined' || wecharOpenID === 'null') {
                        window.localStorage.setItem('wecharOpenID1', '')
                        let url = window.location.href
                        let link = url.split('#/')[1]
                        console.log(link)
                        router.replace({
                            path: '/',
                            query: {
                                link
                            }
                        })
                    } else {
                        window.sessionStorage.setItem(`userToken`, ``)
                        userLogin(wecharOpenID)
                        return
                    }
            }
        }
        return Promise.reject(error.response.data)   // 返回接口返回的错误信息
    }
)
// 接口维护
export default { 

    // 患者端
    yaeherMessage (data) { // 短信接口
        return instance.post(`${url5002}api/YaeherMessage`, data)
    },
    wXJSTicket (data) { // 微信参数
        return instance.post(`${url5002}api/WXJSTicket`, data)
    },
    acceptWecharStatePage (data) { // 获取聊天列表
        return instance.post(`${url5002}api/AcceptWecharStatePage`, data)
    },
    unReadyWecharList (data) { // 获取聊天条数
        return instance.post(`${url5002}api/UnReadyWecharList`, data)
    },
    acceptWecharStateId (data) { // 获取聊天详情
        return instance.post(`${url5002}api/AcceptWecharStateId`, data)
    },
    sendWechaMessgae (data) { // 发送消息
        return instance.post(`${url5002}api/SendWechaMessgae`, data)
    },
    updateWecharState (data) { // 置顶
        return instance.post(`${url5002}api/UpdateWecharState`, data)
    },
    yaeherPhone (data) { // 电话接口
        return instance.post(`${url5001}api/CreatePhoneReplyRecord`, data)
    },
    phoneRecord (data) { // 电话记录查询
        return instance.post(`${url5001}api/PhoneReplyRecordList`, data)
    },
    wXOAuthPay (data) { // 获取支付参数
        return instance.post(`${url5002}api/WXOAuthPay`, data)
    },
    wXOAuthPayProcessingRelease (data) { // 修改订单为未付款状态
        return instance.post(`${url5000}api/WXOAuthPayProcessingRelease`, data)
    },
    getAppId (data) { // 获取appid接口
        return instance.post(`${url5002}api/SystemConfigsBySystemType`, data)
    },
    yaeherBannerList (data) { // 获取bannerList
        return instance.post(`${url5002}api/YaeherBannerList`, data)
    },
    yaeherLabelListByCode (data) { // 获取labelList
        return instance.post(`${url5002}api/YaeherLabelListByCode`, data)
    },
    tencentCosAccessTokenType (data) { // 获取type参数
        return instance.post(`${url5002}api/TencentCosAccessTokenType`, data)
    },
    loginAll (data) { // 登陆
        return instance.post(`${url5002}api/TokenAuth/Authenticate`, data)
    },
    doctorLables (data) { // 获取病例标签列表
        return instance.post(`${url5002}api/DoctorLables`, data)
    },
    tencentCosAccessToken (data) { // 获取upload参数
        return instance.post(`${url5002}api/TencentCosAccessToken`, data)
    },
    createAttachment (data) { // 上传upload参数
        return instance.post(`${url5002}api/CreateAttachment`, data)
    },
    userLogin (data) { // 用户登陆
        return instance.post(`${url5002}api/Authenticate`, data)
    },
    patientInfo (data) { // 获取用户信息
        return instance.post(`${url5002}api/YaeherUser`, data)
    },
    clinicDoctorsPage (data) { // 获取首页专业分组
        return instance.post(`${url5001}api/YaeherClinicDoctorsPage`, data)
    },
    yaeherClinicDoctors (data) { // 获取医生列表
        return instance.post(`${url5002}api/DoctorSortList`, data)
    },
    yaeherDoctorSearch (data) { // 医生查询
        return instance.post(`${url5001}api/YaeherDoctorSearch`, data)
    },
    lableList (data) { // 获取科室对应标签
        return instance.post(`${url5002}api/LableClinicList`, data)
    },
    doctorSearch (data) { // 医生搜索
        return instance.post(`${url5001}api/YaeherDoctorSearch`, data)
    },
    clinicDoctor (data) { // 医生详情
        return instance.post(`${url5001}api/YaeherClinicDoctor`, data)
    },
    yaeherDoctorById (data) { // 医生简单信息
        return instance.post(`${url5001}api/YaeherDoctorById`, data)
    },
    doctorConsultationRelationList (data) { // 疾病类型列表带其它
        return instance.post(`${url5001}api/DoctorConsultationRelationList`, data)
    },
    doctorPaperPage (data) { // 医生文章列表
        return instance.post(`${url5001}api/DoctorPaperPage`, data)
    },
    releaseManagePage (data) { // 看文章
        return instance.post(`${url5002}api/ReleaseManagePage`, data)
    },
    doctorPaperById (data) { // 医生文章详情
        return instance.post(`${url5001}api/DoctorPaperById`, data)
    },
    releaseManageById (data) { // 医生文章详情
        return instance.post(`${url5002}api/ReleaseManageById`, data)
    },
    patientDoctor (data) { // 收藏取消医生
        return instance.post(`${url5000}api/YaeherPatientDoctor`, data)
    },
    createConsultation (data) { // 提交咨询
        return instance.post(`${url5000}api/CreateConsultation`, data)
    },
    consultationPaid (data) { // 提交支付
        return instance.post(`${url5000}api/ConsultationPaid`, data)
    },
    consultationPage (data) { // 我的咨询列表
        return instance.post(`${url5000}api/ConsultationPage`, data)
    },
    consultationDetail (data) { // 我的咨询详情
        return instance.post(`${url5000}api/ConsultationDetail`, data)
    },
    updateConsultation (data) { // 咨询修改
        return instance.post(`${url5000}api/UpdateConsultation`, data)
    },
    consultationReplyParameter (data) { // 咨询追问参数
        return instance.post(`${url5002}api/ConsultationReplyParameter`, data)
    },
    createConsultationReply (data) { // 咨询追问
        return instance.post(`${url5000}api/CreateConsultationReply`, data)
    },
    createReturnVisit (data) { // 咨询回访
        return instance.post(`${url5000}api/CreateReturnVisit`, data)
    },
    deleteConsultation (data) { // 删除咨询
        return instance.post(`${url5000}api/DeleteConsultation`, data)
    },
    refundManageType (data) { // 患者退单理由
        return instance.post(`${url5002}api/RefundManageType`, data)
    },
    createRefundManage (data) { // 咨询退单患者
        return instance.post(`${url5000}api/CreateRefundManage`, data)
    },
    consultationEvaluationReson (data) { // 评价理由
        return instance.post(`${url5002}api/ConsultationEvaluationReson`, data)
    },
    createConsultationEvaluation (data) { // 评价提交
        return instance.post(`${url5000}api/CreateConsultationEvaluation`, data)
    },
    consultationEvaluationById (data) { // 评价详情
        return instance.post(`${url5000}api/ConsultationEvaluationById`, data)
    },
    leaguerInfoPage (data) { // 我的咨询人列表
        return instance.post(`${url5000}api/LeaguerInfoPage`, data)
    },
    updateYaeherUser (data) { // 个人信息修改
        return instance.post(`${url5002}api/UpdateYaeherUser`, data)
    },
    leaguerInfoById (data) { // 咨询人信息详情
        return instance.post(`${url5000}api/LeaguerInfoById`, data)
    },
    updateLeaguerInfo (data) { // 咨询人信息修改
        return instance.post(`${url5000}api/UpdateLeaguerInfo`, data)
    },
    createLeaguerInfo (data) { // 添加咨询人
        return instance.post(`${url5000}api/CreateLeaguerInfo`, data)
    },
    deleteLeaguerInfo (data) { // 删除咨询人
        return instance.post(`${url5000}api/DeleteLeaguerInfo`, data)
    },
    questionReleasePage (data) { // 看问答列表、搜索问答
        return instance.post(`${url5002}api/QuestionReleasePage`, data)
    },
    questionReleaseById (data) { // 看问答详情
        return instance.post(`${url5002}api/QuestionReleaseById`, data)
    },
    questionReleasepraise (data) { // 看问答点赞
        return instance.post(`${url5002}api/QuestionReleasepraise`, data)
    },
    leaguerInfoList (data) { // 我的咨询人列表
        return instance.post(`${url5000}api/LeaguerInfoList`, data)
    },
    yaeherPatientDoctorPage (data) { // 我的医生列表
        return instance.post(`${url5000}api/YaeherPatientDoctorPage`, data)
    },
    // ---- 医生端 ----
    createQuickReplyD (data) { // 创建快捷回复
        return instance.post(`${url5002}api/CreateQuickReply`, data)
    },
    updateQuickReplyD (data) { // 修改快捷回复
        return instance.post(`${url5002}api/UpdateQuickReply`, data)
    },
    deleteQuickReplyD (data) { // 删除快捷回复
        return instance.post(`${url5002}api/DeleteQuickReply`, data)
    },
    quickReplyPageD (data) { // 快捷回复列表
        return instance.post(`${url5002}api/QuickReplyPage`, data)
    },
    quickReplyListD (data) { // 快捷回复列表2
        return instance.post(`${url5002}api/QuickReplyList`, data)
    },
    quickReplyByIdD (data) { // 快捷回复详情
        return instance.post(`${url5002}api/QuickReplyById`, data)
    },
    getReplyMemoryD (data) { // 获取回复缓存
        return instance.post(`${url5002}api/GetReplyMemory`, data)
    },
    consultReplyMemoryD (data) { // 更新回复缓存
        return instance.post(`${url5002}api/ConsultReplyMemory`, data)
    },
    removeConsultReplyMemoryD (data) { // 删除回复缓存
        return instance.post(`${url5002}api/RemoveConsultReplyMemory`, data)
    },
    consultationPageD (data) { // 咨询列表
        return instance.post(`${url5001}api/ConsultationPage`, data)
    },
    consultationDetailD (data) { // 咨询详情、病例夹详情
        return instance.post(`${url5001}api/ConsultationDetail`, data)
    },
    createConsultationReplyD (data) { // 回复咨询
        return instance.post(`${url5001}api/CreateConsultationReply`, data)
    },
    createRefundManageD (data) { // 退单
        return instance.post(`${url5001}api/CreateRefundManage`, data)
    },
    doctorRefundManageTypeD (data) { // 医生退单原因
        return instance.post(`${url5001}api/DoctorRefundManageType`, data)
    },
    collectConsultation (data) { // 搜查病例
        return instance.post(`${url5001}api/CollectConsultation`, data)
    },
    doctorRulesPage (data) { // 指南列表
        return instance.post(`${url5002}api/DoctorRulesPage`, data)
    },
    doctorRulesById (data) { // 指南详情
        return instance.post(`${url5002}api/DoctorRulesById`, data)
    },
    yaeherDoctorD (data) { // 医生基本资料
        return instance.post(`${url5001}api/YaeherDoctor`, data)
    },
    updateYaeherDoctorD (data) { // 医生基本资料修改
        return instance.post(`${url5001}api/UpdateYaeherDoctor`, data)
    },
    updateYaeherDoctorResumeD (data) { // 简介修改
        return instance.post(`${url5001}api/UpdateYaeherDoctorResume`, data)
    },
    userAuthenticationD (data) { // 修改电话号码
        return instance.post(`${url5002}api/UserAuthentication`, data)
    },
    lablePageD (data) { // 病种列表
        return instance.post(`${url5002}api/LablePage`, data)
    },
    createLableD (data) { // 添加病种
        return instance.post(`${url5002}api/CreateLable`, data)
    },
    deleteLableD (data) { // 删除病种
        return instance.post(`${url5001}api/DeleteDoctorRelation`, data)
    },
    doctorEmploymentPageD (data) { // 执业经历列表
        return instance.post(`${url5001}api/DoctorEmploymentPage`, data)
    },
    createDoctorEmploymentD (data) { // 添加执业经历
        return instance.post(`${url5001}api/CreateDoctorEmployment`, data)
    },
    updateDoctorEmploymentD (data) { // 修改执业经历
        return instance.post(`${url5001}api/UpdateDoctorEmployment`, data)
    },
    doctorEmploymentByIdD (data) { // 执业经历详情
        return instance.post(`${url5001}api/DoctorEmploymentById`, data)
    },
    deleteDoctorEmploymentD (data) { // 删除执业经历
        return instance.post(`${url5001}api/DeleteDoctorEmployment`, data)
    },
    createAttachmentD (data) { // 身份证信息提交
        return instance.post(`${url5002}api/CreateAttachment`, data)
    },
    doctorFileApplyD (data) { // 提交信息到后台
        return instance.post(`${url5001}api/DoctorFileApply`, data)
    },
    yaeherDoctorAuthTypeD (data) { // 获取认证类型
        return instance.post(`${url5001}api/YaeherDoctorAuthType`, data)
    },
    updateYaeherDoctorD1 (data) { // 提交认证类型
        return instance.post(`${url5001}api/UpdateYaeherDoctorByDoctor`, data)
    },
    attachmentListD (data) { // 身份证信息获取
        return instance.post(`${url5002}api/AttachmentList`, data)
    },
    doctorFileApplyListD (data) { // 身份证信息获取
        return instance.post(`${url5001}api/DoctorFileApplyList`, data)
    },
    updateYaeherDoctorByDoctorD (data) { // 提交认证全部信息
        return instance.post(`${url5001}api/UpdateYaeherDoctorByDoctor`, data)
    },
    yaeherUserPaymentPageD (data) { // 收款方式列表
        return instance.post(`${url5002}api/YaeherUserPaymentPage`, data)
    },
    updateYaeherUserPaymentD (data) { // 切换,修改收款方式
        return instance.post(`${url5002}api/UpdateYaeherUserPayment`, data)
    },
    createYaeherUserPaymentD (data) { // 新增方式列表
        return instance.post(`${url5002}api/CreateYaeherUserPayment`, data)
    },
    yaeherUserPaymentTypeD (data) { // 获取收款类型
        return instance.post(`${url5002}api/YaeherUserPaymentType`, data)
    },
    serviceMoneyListTypeD (data) { // 获取服务类型
        return instance.post(`${url5001}api/ServiceMoneyListType`, data)
    },
    serviceMoneyListD (data) { // 我的服务
        return instance.post(`${url5001}api/ServiceMoneyList`, data)
    },
    serviceMoneyListByIDD (data) { // 获取服务类型详情
        return instance.post(`${url5001}api/ServiceMoneyListByID`, data)
    },
    createServiceMoneyListD (data) { // 服务创建
        return instance.post(`${url5001}api/CreateServiceMoneyList`, data)
    },
    updateServiceMoneyListD (data) { // 服务修改
        return instance.post(`${url5001}api/UpdateServiceMoneyList`, data)
    },
    collectConsultationPageD (data) { // 病例夹列表
        return instance.post(`${url5001}api/CollectConsultationPage`, data)
    },
    createDoctorPaperD (data) { // 发布病例
        return instance.post(`${url5001}api/CreateDoctorPaper`, data)
    },
    doctorPaperTypeD (data) { // 文章类型
        return instance.post(`${url5001}api/DoctorPaperType`, data)
    },
    doctorPaperPageD (data) { // 文章列表
        return instance.post(`${url5001}api/DoctorPaperPage`, data)
    },
    doctorPaperByIdD (data) { // 文章详情
        return instance.post(`${url5001}api/DoctorPaperById`, data)
    },
    createDoctorPaperD (data) { // 发布文章
        return instance.post(`${url5001}api/CreateDoctorPaper`, data)
    },
    doctorSchedulingTypeD (data) { // 门诊排班类型获取
        return instance.post(`${url5001}api/DoctorSchedulingType`, data)
    },
    createDoctorSchedulingD (data) { // 添加门诊排班类型
        return instance.post(`${url5001}api/CreateDoctorScheduling`, data)
    },
    doctorSchedulingPageD (data) { // 门诊排班列表
        return instance.post(`${url5001}api/DoctorSchedulingPage`, data)
    },
    doctorSchedulingByIDD (data) { // 门诊排班详情
        return instance.post(`${url5001}api/DoctorSchedulingByID`, data)
    },
    updateDoctorSchedulingD (data) { // 修改排班详情
        return instance.post(`${url5001}api/UpdateDoctorScheduling`, data)
    },
    deleteDoctorSchedulingD (data) { // 删除门诊排班
        return instance.post(`${url5001}api/DeleteDoctorScheduling`, data)
    },
    consultationEvaluationPageD (data) { // 评价列表
        return instance.post(`${url5001}api/ConsultationEvaluationPage`, data)
    },
    consultationEvaluationDetailD (data) { // 评价总数
        return instance.post(`${url5001}api/ConsultationEvaluationDetail`, data)
    },
    createYaeherDoctorD (data) { // 医生注册
        return instance.post(`${url5001}api/CreateYaeherDoctor`, data)
    },
    clinicListD (data) { // 获取科室列表
        return instance.post(`${url5002}api/DoctorOutClinicList`, data)
    },
    createQualityCommitteeRegisterD (data) { // 提交质控委员申请
        return instance.post(`${url5002}api/CreateQualityCommitteeRegister`, data)
    },
    qualityCommitteeByUserD (data) { // 提交质控委员处理状态
        return instance.post(`${url5002}api/QualityCommitteeByUser`, data)
    },
    yaeherPatientParameterListD (data) { // 申请质控委员参数类型
        return instance.post(`${url5002}api/YaeherMobileParameterList`, data)
    },
    qualityControlManagePageD (data) { // 申诉审核处理列表
        return instance.post(`${url5001}api/QualityControlManagePage`, data)
    },
    qualityControlManageDetailD (data) { // 申诉审核处理详情
        return instance.post(`${url5001}api/QualityControlManageDetail`, data)
    },
    qualityControlManageD (data) { // 提交申诉评价内容
        return instance.post(`${url5001}api/QualityControlManage`, data)
    },
    doctorClinicApplyPageD (data) { // 科室列表
        return instance.post(`${url5001}api/DoctorClinicApplyPage`, data)
    },
    doctorClinicApplyOutD (data) { // 科室列表新
        return instance.post(`${url5001}api/DoctorClinicApplyOut`, data)
    },
    doctorClinicApplyD (data) { // 新增科室
        return instance.post(`${url5001}api/DoctorClinicApply`, data)
    },
    doctorClinicApplyByIdD (data) { // 科室认证详情
        return instance.post(`${url5001}api/DoctorClinicApplyById`, data)
    },
    revenueTotalD (data) { // 我的收入排行
        return instance.post(`${url5001}api/RevenueTotal`, data)
    },
    orderManageListD (data) { // 我的收入排行列表
        return instance.post(`${url5001}api/TotalOrderManagePage`, data)
    },
    // ---------- 质控端 ---------
    qualityEvaluationTotalC (data) { // 评价统计
        return instance.post(`${url5002}api/QualityEvaluationTotal`, data)
    },
    qualityEvaluationListC (data) { // 评价列表
        return instance.post(`${url5002}api/QualityEvaluationPage`, data)
    },
    updateConsultationEvaluationC (data) { // 质控打分
        return instance.post(`${url5001}api/UpdateConsultationEvaluation`, data)
    },
    consultationEvaluationByIdC (data) { // 质控打分
        return instance.post(`${url5001}api/ConsultationEvaluationById`, data)
    },
    qualityCommitteePageC (data) { // 质控委员列表
        return instance.post(`${url5002}api/QualityCommitteePage`, data)
    },
    createQualityControlManageC (data) { // 转给质控委员
        return instance.post(`${url5001}api/CreateQualityControlManage`, data)
    },
    qualityConsultationPageC (data) { // 申诉管理列表
        return instance.post(`${url5001}api/QualityConsultationPage`, data)
    },
    celeteQualityCommitteeC (data) { // 删除质控委员
        return instance.post(`${url5002}api/DeleteQualityCommittee`, data)
    },
    createQualityCommitteeC (data) { // 设置质控委员
        return instance.post(`${url5002}api/CreateQualityCommittee`, data)
    },
    qualityCommitteeRegisterPageC (data) { // 申请质控委员列表
        return instance.post(`${url5002}api/QualityCommitteeRegisterPage`, data)
    },
    qualityCommitteeRegisterByIdC (data) { // 申请质控委员申请详情
        return instance.post(`${url5002}api/QualityCommitteeRegisterById`, data)
    },
    qualityCommittee (data) { // 申请质控委员提交审核
        return instance.post(`${url5002}api/QualityCommittee`, data)
    },
    qualityDoctorRankingC (data) { // 医生排行
        return instance.post(`${url5002}api/QualityDoctorRanking`, data)
    },
    qualityYaeherDoctorSearchC (data) { // 质控端新医生查询
        return instance.post(`${url5001}api/QualityYaeherDoctorSearch`, data)
    },
    qalityReturnReportC (data) { // 质控端退单率统计
        return instance.post(`${url5002}api/QualityReturnReport`, data)
    },
    // -----------管理端----------
    yaeherDoctorPageA (data) { // 管理端医生审核列表
        return instance.post(`${url5001}api/YaeherDoctorPage`, data)
    },
    authCheckYaeherDoctorA (data) { // 审核医生信息
        return instance.post(`${url5001}api/AuthCheckYaeherDoctor`, data)
    },
    refundManagePageA (data) { // 管理端退单列表
        return instance.post(`${url5001}api/RefundManagePage`, data)
    },
    refundManageByIdA (data) { // 管理端退单详情
        return instance.post(`${url5001}api/RefundManageById`, data)
    },
    updateRefundManageA (data) { // 管理端退单审核
        return instance.post(`${url5001}api/UpdateRefundManage`, data)
    },
    adminConsultationReportA (data) { // 管理端订单统计
        return instance.post(`${url5002}api/AdminConsultationReport`, data)
    },
    adminFlowReportA (data) { // 管理端流量统计
        return instance.post(`${url5002}api/AdminFlowReport`, data)
    },
    doctorParaSetListA (data) { // 管理端参数列表
        return instance.post(`${url5002}api/DoctorParaSetList`, data)
    },
    doctorParaSetByIdA (data) { // 管理端参数详情
        return instance.post(`${url5002}api/DoctorParaSetById`, data)
    },
    updateDoctorParaSetA (data) { // 管理端参数修改
        return instance.post(`${url5002}api/UpdateDoctorParaSet`, data)
    },
    adminIncomeReportC (data) { // 管理端收入上部
        return instance.post(`${url5002}api/AdminIncomeReport`, data)
    },
    adminIncomeDetailReportC (data) { // 管理端收入明细
        return instance.post(`${url5002}api/AdminIncomeDetailReport`, data)
    },
    // -----------客服端----------
    
    customerServiceYaeherDoctorByIdC (data) { // 客服端医生详情
        return instance.post(`${url5001}api/CustomerServiceYaeherDoctorById`, data)
    },
    clinicPageC (data) { // 客服端科室列表
        return instance.post(`${url5002}api/ClinicList`, data)
    },
    clinicByIdC (data) { // 客服端科室详情
        return instance.post(`${url5002}api/ClinicById`, data)
    },
    updateClinicC (data) { // 客服端科室修改
        return instance.post(`${url5002}api/UpdateClinic`, data)
    },
    createClinicC (data) { // 客服端科室添加
        return instance.post(`${url5002}api/CreateClinic`, data)
    },
    deleteClinicC (data) { // 客服端科室删除
        return instance.post(`${url5002}api/DeleteClinic`, data)
    },
    updateDoctorOnlineRecordC (data) { // 设置医生上下线
        return instance.post(`${url5002}api/UpdateDoctorOnlineRecord`, data)
    },
    deleteYaeherDoctorC (data) { // 删除医生
        return instance.post(`${url5001}api/DeleteYaeherDoctor`, data)
    },
    doctorClinicApplyPageC (data) { // 客服获取医生科室审核列表
        return instance.post(`${url5001}api/DoctorClinicApplyPage`, data)
    },
    doctorClinicListByDoctorIDC (data) { // 客服获取医生科室列表
        return instance.post(`${url5001}api/DoctorClinicListByDoctorID`, data)
    },
    createDoctorClinicC (data) { // 客服分配科室
        return instance.post(`${url5001}api/CreateDoctorClinic`, data)
    },
    checkDoctorClinicC (data) { // 分配科室检查
        return instance.post(`${url5001}api/CheckDoctorClinic`, data)
    },
    doctorClinicApplyByIdC (data) { // 审核详情
        return instance.post(`${url5001}api/DoctorClinicApplyById`, data)
    },
    updateDoctorClinicApplyC (data) { // 进行审核
        return instance.post(`${url5001}api/UpdateDoctorClinicApply`, data)
    },
    joinWecharStateC (data) { // 加入客服
        return instance.post(`${url5002}api/JoinWecharState`, data)
    },
}
