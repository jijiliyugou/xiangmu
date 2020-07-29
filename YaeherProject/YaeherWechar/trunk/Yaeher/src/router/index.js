import Vue from 'vue'
import Router from 'vue-router'

const indexPatient = resolve => require(['views/patient/index'], resolve)
const doctorListPatient = resolve => require(['views/patient/doctorList'], resolve)
const doctorDetailPatient = resolve => require(['views/patient/doctorDetail'], resolve)
const Agreement = resolve => require(['views/patient/agreement'], resolve)
const agreement1 = resolve => require(['views/patient/agreement1'], resolve)
const Consultation = resolve => require(['views/patient/consultation'], resolve)
const patientSuccess = resolve => require(['views/patient/success'], resolve)
const articleList = resolve => require(['views/patient/articleList'], resolve)
const articleDetail = resolve => require(['views/patient/articleDetail'], resolve)
const userPatient = resolve => require(['views/patient/user'], resolve)
const patientInfo = resolve => require(['views/patient/userInfo'], resolve)
const patientInfoDetails = resolve => require(['views/patient/infoDetail'], resolve)
const Coupon = resolve => require(['views/patient/coupon'], resolve)
const userDoctor = resolve => require(['views/patient/userDoctor'], resolve)
const userNumber = resolve => require(['views/patient/userNumber'], resolve)
const numberDetail = resolve => require(['views/patient/numberDetail'], resolve)
const addNumber = resolve => require(['views/patient/addNumber'], resolve)
const lookQuestion = resolve => require(['views/patient/lookQuestion'], resolve)
const lookArticle = resolve => require(['views/patient/lookArticle'], resolve)
const questionDetail = resolve => require(['views/patient/questionDetail'], resolve)
const questionSearch = resolve => require(['views/patient/questionSearch'], resolve)
const userRecord = resolve => require(['views/patient/userRecord'], resolve)
const recordDetail = resolve => require(['views/patient/recordDetail'], resolve)
const Evaluate = resolve => require(['views/patient/evaluate'], resolve)
const evaluateDetail = resolve => require(['views/patient/evaluateDetail'], resolve)
const Chargeback = resolve => require(['views/patient/chargeback'], resolve)
const Chat = resolve => require(['views/patient/chat'], resolve)
const doctorScheduling = resolve => require(['views/patient/doctorScheduling'], resolve)
const Search = resolve => require(['views/patient/search'], resolve)
const Version = resolve => require(['views/patient/version'], resolve)
const Login = resolve => require(['views/patient/login'], resolve)
const shareCode = resolve => require(['views/patient/shareCode'], resolve)

const agreementLogin = resolve => require(['views/doctor/agreementLogin'], resolve)
const Register = resolve => require(['views/doctor/register'], resolve)
const indexDoctor = resolve => require(['views/doctor/index'], resolve)
const orderDetail = resolve => require(['views/doctor/orderDetail'], resolve)
const orderLook = resolve => require(['views/doctor/orderLook'], resolve)
const patientOrder = resolve => require(['views/doctor/patientOrder'], resolve)
const patientOrderDetail = resolve => require(['views/doctor/patientOrderDetail'], resolve)
const Guide = resolve => require(['views/doctor/guide'], resolve)
const guideDetail = resolve => require(['views/doctor/guideDetail'], resolve)
const doctorUser = resolve => require(['views/doctor/user'], resolve)
const Avatar = resolve => require(['views/doctor/avatar'], resolve)
const qrCode = resolve => require(['views/doctor/qrCode'], resolve)
const doctorInfoList = resolve => require(['views/doctor/infoList'], resolve)
const basicInfo = resolve => require(['views/doctor/basicInfo'], resolve)
const Introduce = resolve => require(['views/doctor/introduce'], resolve)
const Disease = resolve => require(['views/doctor/disease'], resolve)
const addDisease = resolve => require(['views/doctor/addDisease'], resolve)
const Experience = resolve => require(['views/doctor/experience'], resolve)
const addExperience = resolve => require(['views/doctor/addExperience'], resolve)
const Authentication = resolve => require(['views/doctor/authentication'], resolve)
const Authentication1 = resolve => require(['views/doctor/authentication1'], resolve)
const Authentication2 = resolve => require(['views/doctor/authentication2'], resolve)
const Authentication3 = resolve => require(['views/doctor/authentication3'], resolve)
const AuthenticationClinic = resolve => require(['views/doctor/AuthenticationClinic'], resolve)
const Server = resolve => require(['views/doctor/server'], resolve)
const setServer = resolve => require(['views/doctor/setserver'], resolve)
const Arrange = resolve => require(['views/doctor/arrange'], resolve)
const addArrange = resolve => require(['views/doctor/addArrange'], resolve)
const paymentTerm = resolve => require(['views/doctor/paymentTerm'], resolve)
const alterCard = resolve => require(['views/doctor/alterCard'], resolve)
const Income = resolve => require(['views/doctor/income'], resolve)
const incomeList = resolve => require(['views/doctor/incomeList'], resolve)
const Ranking = resolve => require(['views/doctor/ranking'], resolve)
const administrativeOffice = resolve => require(['views/doctor/administrativeOffice'], resolve)
const alterOffice = resolve => require(['views/doctor/alterOffice'], resolve)
const applyControl = resolve => require(['views/doctor/applyControl'], resolve)
const doctorArticle = resolve => require(['views/doctor/doctorArticle'], resolve)
const editorArticle = resolve => require(['views/doctor/editorArticle'], resolve)
const illnessCase = resolve => require(['views/doctor/illnessCase'], resolve)
const auditProcess = resolve => require(['views/doctor/auditProcess'], resolve)
const auditDetail = resolve => require(['views/doctor/auditDetail'], resolve)
const evaluateDoctor = resolve => require(['views/doctor/evaluate'], resolve)
const doctorSearch = resolve => require(['views/doctor/doctorSearch'], resolve)
const uploadCase = resolve => require(['views/doctor/uploadCase'], resolve)
const doctorPhone = resolve => require(['views/doctor/doctorPhone'], resolve)
const lookDetail = resolve => require(['views/doctor/lookDetail'], resolve)
const fastReplay = resolve => require(['views/doctor/fastReplay'], resolve)
const addReplay = resolve => require(['views/doctor/addReplay'], resolve)

const indexControl = resolve => require(['views/control/index'], resolve)
const orderControl = resolve => require(['views/control/orderControl'], resolve)
const doctorListControl = resolve => require(['views/control/doctorList'], resolve)
const orderListControl = resolve => require(['views/control/orderList'], resolve)
const auditArticle = resolve => require(['views/control/auditArticle'], resolve)
const controlUser = resolve => require(['views/control/user'], resolve)
const controlReport = resolve => require(['views/control/report'], resolve)
const controlRanking = resolve => require(['views/control/ranking'], resolve)
const controlChargeback = resolve => require(['views/control/chargeback'], resolve)
const controlList = resolve => require(['views/control/controlList'], resolve)
const addControl = resolve => require(['views/control/addControl'], resolve)
const auditControl = resolve => require(['views/control/auditControl'], resolve)
const auditApply = resolve => require(['views/control/auditApply'], resolve)
const complaintManagement = resolve => require(['views/control/complaintManagement'], resolve)
const controlDoctorList = resolve => require(['views/control/controlDoctorList'], resolve)

const indexCustomer = resolve => require(['views/customer/index'], resolve)
const auditDoctor = resolve => require(['views/customer/auditDoctor'], resolve)
const auditDoctor2 = resolve => require(['views/customer/auditDoctor2'], resolve)
const informationList = resolve => require(['views/customer/informationList'], resolve)
const informationDetail = resolve => require(['views/customer/informationDetail'], resolve)
const customerUser = resolve => require(['views/customer/user'], resolve)
const customerOrder = resolve => require(['views/customer/customerOrder'], resolve)
const customerDetail = resolve => require(['views/customer/customerDetail'], resolve)
const manageDoctor = resolve => require(['views/customer/manageDoctor'], resolve)
const selectClinic = resolve => require(['views/customer/selectClinic'], resolve)
const Subject = resolve => require(['views/customer/subject'], resolve)
const addSubject = resolve => require(['views/customer/addSubject'], resolve)
const auditClinicList = resolve => require(['views/customer/auditClinicList'], resolve)
const auditClinicDetail= resolve => require(['views/customer/auditClinicDetail'], resolve)

const indexAdmin = resolve => require(['views/admin/index'], resolve)
const adminUser = resolve => require(['views/admin/user'], resolve)
const adminEvaluate = resolve => require(['views/admin/adminEvaluate'], resolve)
const Financing = resolve => require(['views/admin/financing'], resolve)
const adminDoctor = resolve => require(['views/admin/adminDoctor'], resolve)
const chargebackList = resolve => require(['views/admin/chargebackList'], resolve)
const chargebackDetail = resolve => require(['views/admin/chargebackDetail'], resolve)
const orderStatistics = resolve => require(['views/admin/orderStatistics'], resolve)
const incomeStatistics = resolve => require(['views/admin/incomeStatistics'], resolve)
const flowStatistics = resolve => require(['views/admin/flowStatistics'], resolve)
const permissionSetting = resolve => require(['views/admin/permissionSetting'], resolve)
const addAdmin = resolve => require(['views/admin/addAdmin'], resolve)
const systemSetup = resolve => require(['views/admin/systemSetup'], resolve)
const systemDetail = resolve => require(['views/admin/systemDetail'], resolve)
const timeMouth = resolve => require(['views/admin/timeMouth'], resolve)
const timeDay = resolve => require(['views/admin/timeDay'], resolve)


Vue.use(Router)

export default new Router({
  routes: [
    {            
      path: '/', // 登陆
      name: 'Login',
      component: Login,
      meta: { keepAlive: false }
    },
    {            
      path: '/share-code', // 公众号二维码
      name: 'shareCode',
      component: shareCode,
      meta: { keepAlive: false }
    },
    {            // ------ 患者端 ------
      path: '/index-patient', // 首页
      name: 'indexPatient',
      component: indexPatient,
      meta: { keepAlive: true }
    },
    { 
      path: '/doctor-list-patient', // 医生列表
      name: 'doctorListPatient',
      component: doctorListPatient,
      meta: { keepAlive: true }
    },
    {
      path: '/doctor-detail-patient', // 医生详情
      name: 'doctorDetailPatient',
      component: doctorDetailPatient,
      meta: { keepAlive: false }
    },
    {
      path: '/agreement', // 协议
      name: 'Agreement',
      component: Agreement,
      meta: { keepAlive: false },
      children: [
        {
          path: '/agreement1', // 协议
          name: 'agreement1',
          component: agreement1,
          meta: { keepAlive: false }
        },
      ]
    },
    {
      path: '/consultation', // 填写咨询内容
      name: 'Consultation',
      component: Consultation,
      meta: { keepAlive: false }
    },
    {
      path: '/patient-success', // 咨询提交成功
      name: 'patientSuccess',
      component: patientSuccess,
      meta: { keepAlive: false }
    },
    {
      path: '/article-list', // 文章列表
      name: 'articleList',
      component: articleList,
      meta: { keepAlive: false }
    },
    {
      path: '/article-detail', // 文章详情
      name: 'articleDetail',
      component: articleDetail,
      meta: { keepAlive: false }
    },
    {
      path: '/user-patient', // 个人中心
      name: 'userPatient',
      component: userPatient,
      meta: { keepAlive: false }
    },
    {
      path: '/patient-info', // 个人信息
      name: 'patientInfo',
      component: patientInfo,
      meta: { keepAlive: false }
    },
    {
      path: '/patient-info-detail', // 个人信息详情
      name: 'patientInfoDetails',
      component: patientInfoDetails,
      meta: { keepAlive: false }
    },
    {
      path: '/coupon', // 优惠券-个人账户
      name: 'Coupon',
      component: Coupon,
      meta: { keepAlive: false }
    },
    {
      path: '/user-doctor', // 我的医生
      name: 'userDoctor',
      component: userDoctor,
      meta: { keepAlive: true }
    },
    {
      path: '/user-number', // 我的成员
      name: 'userNumber',
      component: userNumber,
      meta: { keepAlive: false }
    },
    {
      path: '/add-number', // 添加成员
      name: 'addNumber',
      component: addNumber,
      meta: { keepAlive: false }
    },
    {
      path: '/number-detail', // 成员详情
      name: 'numberDetail',
      component: numberDetail,
      meta: { keepAlive: false }
    },
    {
      path: '/look-article', // 看文章
      name: 'lookArticle',
      component: lookArticle,
      meta: { keepAlive: true }
    },
    {
      path: '/look-question', // 看问答
      name: 'lookQuestion',
      component: lookQuestion,
      meta: { keepAlive: true }
    },
    {
      path: '/question-detail', // 问答详情
      name: 'questionDetail',
      component: questionDetail,
      meta: { keepAlive: false }
    },
    {
      path: '/question-search', // 搜索问答
      name: 'questionSearch',
      component: questionSearch,
      meta: { keepAlive: false }
    },
    {
      path: '/user-record', // 我的咨询
      name: 'userRecord',
      component: userRecord,
      meta: { keepAlive: false }
    },
    {
      path: '/record-detail', // 咨询详情
      name: 'recordDetail',
      component: recordDetail,
      meta: { keepAlive: false }
    },
    {
      path: '/evaluate', // 评价
      name: 'Evaluate',
      component: Evaluate,
      meta: { keepAlive: false }
    },
    {
      path: '/evaluate-detail', // 评价查看
      name: 'evaluateDetail',
      component: evaluateDetail,
      meta: { keepAlive: false }
    },
    {
      path: '/chargeback', // 退单
      name: 'Chargeback',
      component: Chargeback,
      meta: { keepAlive: false }
    },
    {
      path: '/chat', // 联系客服
      name: 'Chat',
      component: Chat,
      meta: { keepAlive: false }
    },
    {
      path: '/search', // 搜索
      name: 'Search',
      component: Search,
      meta: { keepAlive: false }
    },
    {
      path: '/doctor-scheduling', // 门诊排班
      name: 'doctorScheduling',
      component: doctorScheduling,
      meta: { keepAlive: false }
    },
    {
      path: '/version', // 版本
      name: 'Version',
      component: Version,
      meta: { keepAlive: false }
    },
    {             // ------ 医生端 ------
      path: '/register', // 医生注册
      name: 'Register',
      component: Register,
      meta: { keepAlive: false }
    },
    {                    
      path: '/agreement-login', // 认证协议
      name: 'agreementLogin',
      component: agreementLogin,
      meta: { keepAlive: false }
    },
    {                    
      path: '/index-doctor', // 待办
      name: 'indexDoctor',
      component: indexDoctor,
      meta: { keepAlive: false }
    },
    {                   
      path: '/order-detail', // 待办咨询详情
      name: 'orderDetail',
      component: orderDetail,
      meta: { keepAlive: false },
      children: [
        {
          path: '/fast-replay', 
          name: 'fastReplay',
          component: fastReplay,
          meta: { keepAlive: false },
          children: [
            {
              path: '/add-replay', 
              name: 'addReplay',
              component: addReplay,
              meta: { keepAlive: false }
              
            } 
          ]
        } 
      ]
    },
    {                   
      path: '/order-look', // 咨询详情查看
      name: 'orderLook',
      component: orderLook,
      meta: { keepAlive: false }
    },
    {                   
      path: '/patient-order', // 患者历史咨询记录
      name: 'patientOrder',
      component: patientOrder,
      meta: { keepAlive: false }
    },
    {                   
      path: '/patient-order-detail', // 患者历史咨询记录详情
      name: 'patientOrderDetail',
      component: patientOrderDetail,
      meta: { keepAlive: false }
    },
    {                   
      path: '/doctor-search', // 医生搜索模块
      name: 'doctorSearch',
      component: doctorSearch,
      meta: { keepAlive: false }
    },
    {                   
      path: '/guide', // 操作指南
      name: 'Guide',
      component: Guide,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/guide-detail', // 操作指南详情
      name: 'guideDetail',
      component: guideDetail,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/doctor-user', // 个人中心
      name: 'doctorUser',
      component: doctorUser,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/doctor-info-list', // 我的资料列表
      name: 'doctorInfoList',
      component: doctorInfoList,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/avatar', // 我的头像
      name: 'Avatar',
      component: Avatar,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/basic-info', // 基本资料
      name: 'basicInfo',
      component: basicInfo,
      meta: { keepAlive: true } 
    },
    {                   
      path: '/doctor-phone', // 基本资料
      name: 'doctorPhone',
      component: doctorPhone,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/introduce', // 简介
      name: 'Introduce',
      component: Introduce,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/disease', // 病种
      name: 'Disease',
      component: Disease,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/add-disease', // 添加病种
      name: 'addDisease',
      component: addDisease,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/experience', // 病种
      name: 'Experience',
      component: Experience,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/look-detail', // 病种
      name: 'lookDetail',
      component: lookDetail,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/add-experience', // 添加病种
      name: 'addExperience',
      component: addExperience,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/authentication', // 怡禾认证
      name: 'Authentication',
      component: Authentication,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/authentication1', // 怡禾认证1
      name: 'Authentication1',
      component: Authentication1,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/authentication2', // 怡禾认证2
      name: 'Authentication2',
      component: Authentication2,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/authentication3', // 怡禾认证3
      name: 'Authentication3',
      component: Authentication3,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/authentication-clinic', // 科室认证
      name: 'AuthenticationClinic',
      component: AuthenticationClinic,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/server', // 我的服务
      name: 'Server',
      component: Server,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/set-server', // 我的服务
      name: 'setServer',
      component: setServer,
      meta: { keepAlive: true } 
    },
    {                   
      path: '/arrange', // 门诊排班
      name: 'Arrange',
      component: Arrange,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/add-arrange', // 添加排班
      name: 'addArrange',
      component: addArrange,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/payment-term', // 门诊排班
      name: 'paymentTerm',
      component: paymentTerm,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/alter-card', // 修改银行卡
      name: 'alterCard',
      component: alterCard,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/qrCode', // 二维码
      name: 'qrCode',
      component: qrCode,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/income', // 我的收入
      name: 'Income',
      component: Income,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/income-list', // 收入明细
      name: 'incomeList',
      component: incomeList,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/ranking', // 我的排名
      name: 'Ranking',
      component: Ranking,
      meta: { keepAlive: true } 
    },
    {                   
      path: '/administrative-office', // 科室管理
      name: 'administrativeOffice',
      component: administrativeOffice,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/alter-office', // 科室修改
      name: 'alterOffice',
      component: alterOffice,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/apply-control', // 科室修改
      name: 'applyControl',
      component: applyControl,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/doctor-article', // 医生文章
      name: 'doctorArticle',
      component: doctorArticle,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/editor-article', // 文章编辑
      name: 'editorArticle',
      component: editorArticle,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/illness-case', // 文章编辑
      name: 'illnessCase',
      component: illnessCase,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/audit-process', // 质控委员列表
      name: 'auditProcess',
      component: auditProcess,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/audit-detail', // 质控委员审核详情
      name: 'auditDetail',
      component: auditDetail,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/evaluate-doctor', // 我的评论
      name: 'evaluateDoctor',
      component: evaluateDoctor,
      meta: { keepAlive: true } 
    },
    {                   
      path: '/upload-case', // 上传case
      name: 'uploadCase',
      component: uploadCase,
      meta: { keepAlive: false } 
    },
    {                   // ------ 质控端 ------
      path: '/index-control', // 质控评价统计
      name: 'indexControl',
      component: indexControl,
      meta: { keepAlive: true } 
    },
    {                   
      path: '/order-control', // 咨询详情
      name: 'orderControl',
      component: orderControl,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/doctor-list-control', // 医生查看
      name: 'doctorListControl',
      component: doctorListControl,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/order-list-control', // 医生订单
      name: 'orderListControl',
      component: orderListControl,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/audit-article', // 文章审核
      name: 'auditArticle',
      component: auditArticle,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/control-user', // 个人中心
      name: 'controlUser',
      component: controlUser,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/control-report', // 报表统计
      name: 'controlReport',
      component: controlReport,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/control-ranking', // 医生排行
      name: 'controlRanking',
      component: controlRanking,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/control-chargeback', // 退单率
      name: 'controlChargeback',
      component: controlChargeback,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/control-list', // 质控委员列表
      name: 'controlList',
      component: controlList,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/add-control', // 添加质控委员
      name: 'addControl',
      component: addControl,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/audit-control', // 质控委员审核
      name: 'auditControl',
      component: auditControl,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/audit-apply', // 质控申请审核
      name: 'auditApply',
      component: auditApply,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/complaint-management', // 申诉管理
      name: 'complaintManagement',
      component: complaintManagement,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/control-doctor-list', // 质控科室医生列表
      name: 'controlDoctorList',
      component: controlDoctorList,
      meta: { keepAlive: false } 
    },
    {                   // ------ 客服端 ------
      path: '/index-customer', // 客服端首页
      name: 'indexCustomer',
      component: indexCustomer,
      meta: { keepAlive: false } 
    },
    {
      path: '/audit-doctor', // 医生审核
      name: 'auditDoctor',
      component: auditDoctor,
      meta: { keepAlive: false } 
    },
    {
      path: '/audit-doctor2', // 医生审核2
      name: 'auditDoctor2',
      component: auditDoctor2,
      meta: { keepAlive: false }
    },
    {
      path: '/information-list', // 我的消息
      name: 'informationList',
      component: informationList,
      meta: { keepAlive: false }
    },
    {
      path: '/information-detail', // 我的消息
      name: 'informationDetail',
      component: informationDetail,
      meta: { keepAlive: false }
    },
    {
      path: '/customer-user', // 个人中心
      name: 'customerUser',
      component: customerUser,
      meta: { keepAlive: false }
    },
    {
      path: '/customer-order', // 客服订单列表
      name: 'customerOrder',
      component: customerOrder,
      meta: { keepAlive: false }
    },
    {
      path: '/customer-detail', // 客服订单列表
      name: 'customerDetail',
      component: customerDetail,
      meta: { keepAlive: false }
    },
    {
      path: '/manage-doctor', // 医生管理
      name: 'manageDoctor',
      component: manageDoctor,
      meta: { keepAlive: false }
    },
    {
      path: '/select-clinic', // 医生科室分配
      name: 'selectClinic',
      component: selectClinic,
      meta: { keepAlive: false }
    },
    {
      path: '/subject', // 科室管理
      name: 'Subject',
      component: Subject,
      meta: { keepAlive: false }
    },
    {
      path: '/add-subject', // 添加科室
      name: 'addSubject',
      component: addSubject,
      meta: { keepAlive: false }
    },
    {
      path: '/audit-clinic-list', // 审核医生科室列表
      name: 'auditClinicList',
      component: auditClinicList,
      meta: { keepAlive: false }
    },
    {
      path: '/audit-clinic-detail', // 审核医生科室详情
      name: 'auditClinicDetail',
      component: auditClinicDetail,
      meta: { keepAlive: false }
    },
    {                   // ------ 管理端 ------
      path: '/index-admin', // 待办
      name: 'indexAdmin',
      component: indexAdmin,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/admin-user', // 个人中心
      name: 'adminUser',
      component: adminUser,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/financing', // 财务查看
      name: 'Financing',
      component: Financing,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/admin-doctor', // 医生审核列表
      name: 'adminDoctor',
      component: adminDoctor,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/chargeback-list', // 退单列表
      name: 'chargebackList',
      component: chargebackList,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/chargeback-detail', // 退单详情
      name: 'chargebackDetail',
      component: chargebackDetail,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/admin-evaluate', // 评价统计
      name: 'adminEvaluate',
      component: adminEvaluate,
      meta: { keepAlive: true } 
    },
    {                   
      path: '/order-statistics', // 订单统计
      name: 'orderStatistics',
      component: orderStatistics,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/income-statistics', // 收入统计
      name: 'incomeStatistics',
      component: incomeStatistics,
      meta: { keepAlive: true } 
    },
    {                   
      path: '/time-mouth', // 收入统计
      name: 'timeMouth',
      component: timeMouth,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/time-day', // 收入统计
      name: 'timeDay',
      component: timeDay,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/flow-statistics', // 订单统计
      name: 'flowStatistics',
      component: flowStatistics,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/permission-setting', // 权限设置
      name: 'permissionSetting',
      component: permissionSetting,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/add-admin', // 权限添加
      name: 'addAdmin',
      component: addAdmin,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/system-setup', // 系统设置
      name: 'systemSetup',
      component: systemSetup,
      meta: { keepAlive: false } 
    },
    {                   
      path: '/system-detail', // 系统设置详情
      name: 'systemDetail',
      component: systemDetail,
      meta: { keepAlive: false } 
    }
  ]
})
