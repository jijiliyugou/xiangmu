<template>
    <div class="doctorPhone">
        <mt-header fixed title="电话号码维护">
            <a @click="$router.push({ path: '/doctor-info-list'})" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">            
            <div class="patient-info">
                <p class="callHont">注：该电话号码用来进行电话咨询订单的电话回复</p>
                <mt-field label="手机号" placeholder="请输入手机号" type="tel" v-model="phoneNumber"></mt-field>
                <mt-field class="sendCode" label="验证码" v-model="captcha">
                    <mt-button v-if="codeValue > 60" @click="sendCode" readonly  type="primary" class="mint-button--large">发送验证码</mt-button>
                    <mt-button v-if="codeValue <= 60" @click="sendCode" readonly  type="default" class="mint-button--large">{{codeValue}}秒后可重新发送</mt-button>
                </mt-field>
            </div>
            <div class="okCase register-case">
                <mt-button @click="alterPhone" readonly  type="primary" class="mint-button--large">保存</mt-button>
            </div>
        </div>
    </div>
</template>

<script>
import { Toast } from 'mint-ui';
import { createSecret, verifyPrice, timestamp } from 'assets/js/common.js'

export default {
    data () {
        return {
            phoneNumber: '',
            codeValue: 61,
            flag: true,
            captcha: '',
            id: 0
        }
    },
    methods: {
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
                messageType: 'Authentication'
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
        alterPhone() { // 电话号码维护
            const phoneNumber = this.phoneNumber
            const verificationCode = this.captcha
            const id = this.id

             // 校验数据
            let verifyJson = [
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
            if (verifyPriceFlg === 1) return
            this.instance.userAuthenticationD({
                phoneNumber,
                verificationCode,
                id
            })
                .then((response) => {
                    if(response.data.result.code === 200) {
                        Toast('修改成功')
                        this.$router.push({ path: '/doctor-info-list'})
                    }
                    
                })
                .catch((error) => {
                }) 
           
        }
    },
    created () {
        this.phoneNumber = this.$route.query.phoneNumber
        this.id = parseInt(window.sessionStorage.getItem('userId'))
        console.log(this.id)
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.doctorPhone {
    padding: 52px 10px 10px;
    
    .okCase {padding: 0 10px 20px 10px; background: $color-wfont;}
    .register-case {padding: 20px 10px 30px;}
    .mint-cell-text::after {content: '*'; color: $color-red;vertical-align: bottom; line-height: 10px; padding-left: 1px;}
    .patient-info .mint-cell:nth-last-child(1) .mint-cell-text::after {content: '';}
    .mint-field .mint-cell-title {width: 110px;}
    .sendCode .mint-button {font-size: $font-l; height: 38px;}
    .callHont {
        line-height: 18px;
        font-size: $font-l;
        padding: 10px;
        text-align: center;
        color: $color-red;
    }
}

</style>