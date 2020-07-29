<template>
    <div class="register">
        <mt-header fixed title="怡禾健康医生注册"></mt-header>
        <div class="content">            
            <div class="introduce-yi">
                <h1>怡禾健康医生注册</h1>
                <p>怡禾是一个遵循循证医学理念，致力于提供可靠母婴健康咨询的服务平台，目前有来自海内外的400多名医生在提供服务，平均每个医生每天有4-5个咨询，我们会根据咨询需求开放医生上线。</p>
                <p>欢迎在<b>三甲医院工作满五年</b>或<b>主治医生以上职称或在海外执业，认同循证医学理念，有服务意识</b>的医生来注册。</p>
                <p>同时，我们怡禾深圳诊所也已经营业，招聘信息见：<a href="https://mp.weixin.qq.com/s/KzPyDiAuHN4UUgACF0Ea1w">怡禾深圳诊所招聘信息</a>，有意加入的儿保、口腔、皮肤医生可发送简历到：hr@yaeher.com</p>
            </div>
            <!-- <form> -->
                <div class="patient-info">
                    <mt-field label="姓名" placeholder="请输入真实姓名" v-model="doctorName"></mt-field>
                    <mt-field label="身证号码" placeholder="请输入您的身份证号码" v-model="IDCard"></mt-field>
                    <mt-field label="工作医院" placeholder="请输入工作医院" v-model="hospitalName"></mt-field>
                    <mt-field label="所在专科" placeholder="请输入所在专科" v-model="department"></mt-field>
                    <mt-field label="工作年限" placeholder="自执业注册算起" v-model="workYear"></mt-field>
                    <!-- <mt-field label="职称" placeholder="请输入您的职称" v-model="title"></mt-field> -->
                    <div class="illness border-top" @click="selectIllness">
                        <div class="illness-case">
                            <p class="illness-label">职称</p>
                            <p v-if="!title" class="illness-value plValue">请选择职称</p>
                            <p v-if="title" class="illness-value">{{title}}</p>
                        </div>
                    </div>
                    <mt-field label="毕业学校" placeholder="请输入您的毕业学校" v-model="graduateSchool"></mt-field>
                    <mt-radio
                    class="openClose"
                    title="是否相信中医"
                    v-model="isBelieveTCM"
                    :options="options">
                    </mt-radio>

                    <mt-radio
                    class="openClose"
                    title="是否觉得自己有服务意识"
                    v-model="isServiceConscious"
                    :options="options1">
                    </mt-radio>

                    <mt-field label="您的微信号" placeholder="请确认打开了可搜索" v-model="wechatNum"></mt-field>
                    <mt-field label="手机号" placeholder="请输入手机号" type="tel" v-model="phoneNumber"></mt-field>
                    <mt-field class="sendCode" label="验证码" v-model="captcha">
                        <mt-button v-if="codeValue > 60" @click="sendCode" readonly  type="primary" class="mint-button--large">发送验证码</mt-button>
                        <mt-button v-if="codeValue <= 60" @click="sendCode" readonly  type="default" class="mint-button--large">{{codeValue}}秒后可重新发送</mt-button>
                    </mt-field>
                    <mt-field label="推荐人" placeholder="没有可不填" v-model="recommenderName"></mt-field>
                </div>
                <div v-if="btnShow" class="okCase register-case">
                    <mt-button @click="applyRegister" readonly  type="primary" class="mint-button--large">提交</mt-button>
                </div>
            <!-- </form> -->
        </div>
        <mt-picker :slots="dataList2" ref="picker" :showToolbar="true" v-show="show">
            <div @click="selectIllness" class="slots-no">取消</div>
            <div @click="getPickerValue" class="slots-ok">确认</div>
        </mt-picker>
    </div>
</template>

<script>
import { MessageBox } from 'mint-ui';
import { Toast } from 'mint-ui';
import { createSecret, verifyPrice, timestamp } from 'assets/js/common.js'

export default {
    data () {
        return {
            doctorName: '',
            IDCard: '',
            hospitalName: '',
            phoneNumber: '',
            department: '',
            workYear: '',
            title: '',
            codeValue: 61,
            flag: true,
            graduateSchool: '',
            isBelieveTCM: '',
            id: 0,
            isServiceConscious: '',
            wechatNum: '',
            recommenderName: '',
            btnShow: false,
            show: false,
            captcha: '',
            dataList2: [
                {
                    flex: 1,
                    values: ['医师', '主治医师', '副主任医师', '主任医师', '药师', '主管药师', '副主任药师', '主任药师', '临床心理学家', '心理咨询师', '心理治疗师', '临床营养师', '其他'],
                    className: 'slot2',
                    textAlign: 'center'
                }
            ],
            options: [
                {   
                    label: '是',
                    value: '1'
                },
                {
                    label: '否',
                    value: '0'
                }
            ],
            options1: [
                {   
                    label: '是',
                    value: '1'
                },
                {
                    label: '否',
                    value: '0'
                }
            ],
        }
    },
    methods: {
        // okAgreement() {
        //     this.show = false
        // },
        selectIllness(index) { // 呼出咨询人或疾病类型
            this.show = !this.show
            
        },
        getPickerValue() {
                this.show = !this.show
                this.title = this.$refs.picker.getValues()[0]
            
        },
        sendCode() { // 点击发送验证码
            let _this = this
            const phoneNumber = this.phoneNumber
            if(!phoneNumber) {
                Toast('手机号不能为空')
                return
            }
            if (!this.flag) return
            this.flag = false
            _this.codeValue = 60
            _this.sendMessage()
            let interval = window.setInterval(function() {
                if ((_this.codeValue--) <= 0) {
                    _this.codeValue = 61;
                    _this.flag = true;
                    window.clearInterval(interval);
                }
            }, 1000)
        },
        sendMessage() { // 发送验证码
            const phoneNumber = this.phoneNumber
            this.instance.yaeherMessage({
                phoneNumber,
                messageType: 'Verification'
            })
                .then((response) => {
                    if(response.data.result.code === 200) {
                        Toast('发送成功')
                    }
                    
                })
                .catch((error) => {
                    // Toast('发送失败')
                }) 
        },
        applyRegister() { // 医生注册
            const doctorName = this.doctorName
            const IDCard = this.IDCard
            const hospitalName = this.hospitalName
            const phoneNumber = this.phoneNumber
            const department = this.department
            const graduateSchool = this.graduateSchool
            const title = this.title
            let workYear = this.workYear
            let isBelieveTCM = this.isBelieveTCM
            let isServiceConscious = this.isServiceConscious
            const wechatNum = this.wechatNum
            const recommenderName = this.recommenderName
            const verificationCode = this.captcha

             // 校验数据
            let verifyJson = [
                {
                    value: doctorName,
                    msg: '姓名不能为空'
                },
                {
                    value: IDCard,
                    msg: '身份证号不能为空'
                },
                {
                    value: hospitalName,
                    msg: '工作医院不能为空'
                },
                {
                    value: department,
                    msg: '所在专科不能为空'
                },
                {
                    value: workYear,
                    msg: '工作年限不能为空'
                },
                {
                    value: title,
                    msg: '职称不能为空'
                },
                {
                    value: graduateSchool,
                    msg: '毕业学校不能为空'
                },
                {
                    value: isBelieveTCM,
                    msg: '请选择是否相信中医'
                },
                {
                    value: isServiceConscious,
                    msg: '请选择是否有服务意识'
                },
                {
                    value: wechatNum,
                    msg: '微信号不能为空'
                },
                {
                    value: phoneNumber,
                    msg: '电话号码不能为空'
                },
                {
                    value: verificationCode,
                    msg: '验证码不能为空'
                }
            ]
            let verifyPriceFlg = verifyPrice(verifyJson)
            workYear = parseInt(workYear)
            isBelieveTCM = parseInt(isBelieveTCM)
            isServiceConscious = parseInt(isServiceConscious)
            if (verifyPriceFlg === 1) return
            this.instance.createYaeherDoctorD({
                doctorName,
                IDCard,
                hospitalName,
                phoneNumber,
                department,
                workYear,
                isBelieveTCM,
                graduateSchool,
                title,
                isServiceConscious,
                wechatNum,
                recommenderName,
                verificationCode
            })
                .then((response) => {
                    if(response.data.result.code === 200) {
                        MessageBox('提交信息', '提交成功，等待审核')
                        this.getUserInfo()
                    }
                    
                })
                .catch((error) => {
                }) 
           
        },
        getUserInfo() { // 获取用户注册信息
            this.instance.yaeherDoctorById({
                })
                    .then((response) => {
                        let userdetail = response.data.result.item
                        if (userdetail) {
                            // this.show = false
                            this.doctorName = userdetail.doctorName
                            this.hospitalName = userdetail.hospitalName
                            this.department = userdetail.department
                            this.workYear = userdetail.workYear
                            this.phoneNumber = userdetail.phoneNumber
                            this.graduateSchool = userdetail.graduateSchool
                            this.title = userdetail.title
                            let isServiceConscious = userdetail.isServiceConscious
                            let isBelieveTCM = userdetail.isBelieveTCM
                            isServiceConscious ? this.isServiceConscious = '1' : this.isServiceConscious = '0' 
                            isBelieveTCM ? this.isBelieveTCM = '1' : this.isBelieveTCM = '0' 
                            this.wechatNum = userdetail.wechatNum
                            this.recommenderName = userdetail.recommenderName
                            let checkRes = userdetail.checkRes
                            if (checkRes === 'fail') {
                                this.btnShow = true
                                MessageBox('提示信息', '您的资料审核失败，可修改后再提交。')
                            } else {
                                MessageBox('提示信息', '您的资料正在审核中，不可再次提交')
                                this.btnShow = false
                            }
                        } else {
                            // this.show = true
                            this.btnShow = true
                        }
                        
                    })
                    .catch((error) => {
                        // this.show = true
                    }) 
        }
    },
    mounted () {
       this.getUserInfo()     
    },
    created () {
        this.id = parseInt(window.sessionStorage.getItem('userId'))
        console.log(this.id)
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

// .overScroll {overflow: hidden;}
.register {
    padding: 52px 10px 10px;
    .answer {display: flex; font-size: $font-l; padding: 10px 15px; margin: 10px 0; background: $color-wfont;}
    .answer > div {flex: 1;}
    .doctorAn { text-align: right;}
    .answer-name {line-height: 20px;}
    .doctorAn img, .doctorAn span {display: inline-block; vertical-align: middle;}
    .okCase {padding: 0 10px 20px 10px; background: $color-wfont;}
    .register-case {padding: 20px 10px 30px;}
    .illness {background-color: $color-wfont; padding: 0 0 0 10px;}
    .illness-case {display: flex; padding: 10px 0;}
    .illness-label {width: 110px;}
    .illness-label::after {content: '*'; color: $color-red;vertical-align: bottom; line-height: 18px; padding-left: 1px;}
    .introduce-yi {background: $color-wfont; padding: 20px 10px; font-size: $font-l;}
    .introduce-yi h1 {font-size: 20px; text-align: center; padding: 0px 0px 10px; font-weight: 600;}
    .introduce-yi p {padding: 0 0 10px;}
    .introduce-yi a {color: $default-color;}
    .mint-cell-text::after {content: '*'; color: $color-red;vertical-align: bottom; line-height: 10px; padding-left: 1px;}
    .patient-info .mint-cell:nth-last-child(1) .mint-cell-text::after {content: '';}
    .mint-field .mint-cell-title {width: 110px;}
    .sendCode .mint-button {font-size: $font-l; height: 38px;}
    .agreementCase {
        position: fixed;
        top: 42px;
        left: 0%;
        right: 0%;
        bottom: 0%;
        background: #fff;
        padding: 0px 10px 80px;
        .agreementText {
            width: 100%;
            height: 100%;
            padding: 10px 0 0 0;
            overflow: auto;
            -webkit-overflow-scrolling: touch;
            h1 {
                text-align: center;
                font-size: 20px;
                line-height: 36px;
            }
            p {
                font-size: $font-l;
                line-height: 20px;
                color: $color-bfont
            }
            .blod{
                color: $color-afont;
                line-height: 24px;
                padding-top: 3px;
            }
        }
        .agreementHint {
            position: absolute;
            left: 10px;
            right: 10px;
            bottom: 20px;
            display: flex;
            height: 40px;
            div {
                flex: 1;
                text-align: center;
                line-height: 40px;
                button {
                    width: 180px;
                    height: 40px;
                    font-size: $font-xl;
                }
            }
        }
    }
}

</style>