<template>
    <div class="login">
        <!-- <mt-header fixed title="怡禾健康登陆"></mt-header>
        
        <div class="content">            
            <div class="introduce-yi">
                <h1>怡禾健康医生登陆</h1>
                <div class="patient-info">
                    <mt-field label="账户" placeholder="请输入姓名" v-model="userNameOrEmailAddress"></mt-field>
                    <mt-field label="密码" placeholder="请输入工作医院" v-model="password"></mt-field>
                </div>
                <div class="okCase register-case">
                    <mt-button @click="applyRegister" readonly  type="primary" class="mint-button--large">提交</mt-button>
                </div>
                <ul>
                    <li>
                        <router-link to="/index-patient">患者端首页</router-link>
                    </li>
                    <li>
                        <router-link to="/user-patient">患者端个人中心</router-link>
                    </li>
                    <li>
                        <router-link to="/index-doctor">医生端</router-link>
                    </li>
                    <li>
                        <router-link to="/index-control">质控端</router-link>
                    </li>
                    <li>
                        <router-link to="/index-admin">管理端</router-link>
                    </li>
                    <li>
                        <router-link to="/index-customer">客服端</router-link>
                    </li>
                    <li>
                        <router-link to="/look-question">看问答</router-link>
                    </li>
                    <li>
                        <router-link to="/look-article">看文章</router-link>
                    </li>
                    <li>
                        <router-link to="/register">医生注册</router-link>
                    </li>
                    <li>
                        <router-link to="/upload-case">测试页勿点</router-link>
                    </li>
                </ul>
            </div>    
        </div> -->
    </div>
</template>

<script>
import md5 from 'js-md5'
import { Toast } from 'mint-ui';


export default {
    data () {
        return {
            userNameOrEmailAddress: '',
            password: '',
            type: 1,
            id: 0,
            urlHref: ''
        }
    },
    methods: {
        applyRegister() { 
            window.sessionStorage.setItem('userToken', '')
            let type = parseInt(this.type)
            let userNameOrEmailAddress = this.userNameOrEmailAddress
            let password1 = this.password
            let password = md5(password1)
            this.instance.loginAll({
                userNameOrEmailAddress,
                password,
                platform: 'PC'
            })
                .then((response) => {
                    if(response.data.result.code === 200) {
                        let userDate = response.data.result.item
                        window.sessionStorage.setItem('userToken', userDate.accessToken)
                        window.sessionStorage.setItem('userId', userDate.userId)
                        window.sessionStorage.setItem('mobileRoleName', userDate.mobileRoleName)
                        Toast('登陆成功')
                    }
                    
                })
                .catch((error) => {
                })     
           
        },
        getAppIdF() { // 跳转授权链接
            let systemType = 'TencentWechar'
            this.instance.getAppId({
                systemType
            })
                .then((response) => {
                    if(response.data.result.code === 200) {
                        let id = response.data.result.item.appID
                        console.log(`appID=${id}`)
                        let url = `https://open.weixin.qq.com/connect/oauth2/authorize?appid=${id}&redirect_uri=http%3a%2f%2fwechar.yaeherhealth.com%2f%23%2f&response_type=code&scope=snsapi_base&state=123#wechat_redirect`
                        window.location.href = url
                    }
                    
                })
                .catch((error) => {
                })    
        },
        getUserInfo(code1) { // 获取用户信息
            let link1 = window.sessionStorage.getItem('link')
            let wXCode = code1
            console.log(`link1=${link1}`)
            console.log(`wXCode=${wXCode}`)
            this.instance.loginAll({
                wXCode
            })
                .then((response) => {
                    if(response.data.result.code === 200) {
                        let userDate = response.data.result.item
                        console.log(userDate)
                        window.sessionStorage.setItem('userToken', userDate.accessToken)
                        window.sessionStorage.setItem('userId', userDate.userId)
                        window.sessionStorage.setItem('mobileRoleName', userDate.mobileRoleName)
                        window.localStorage.setItem('wecharOpenID1', userDate.wecharOpenID)
                        let link = window.sessionStorage.getItem('link')
                        console.log('linkLogin', link)
                        this.$router.push({ 
                                path: link
                            })
                    }
                    
                })
                .catch((error) => {
                })    
        },
        openIdGetUser() {
            let wecharOpenID = window.localStorage.getItem('wecharOpenID1')
            this.instance.loginAll({
                wecharOpenID
            })
                .then((response) => {
                    if(response.data.result.code === 200) {
                        let userDate = response.data.result.item
                        console.log(userDate)
                        window.sessionStorage.setItem('userToken', userDate.accessToken)
                        window.sessionStorage.setItem('userId', userDate.userId)
                        window.sessionStorage.setItem('mobileRoleName', userDate.mobileRoleName)
                        // window.localStorage.setItem('wecharOpenID', userDate.wecharOpenID)
                        let link = window.sessionStorage.getItem('link')
                        console.log('linkLogin', link)
                        this.$router.push({
                                path: link
                            })
                    }
                    
                })
                .catch((error) => {
                })
        },
        getUrlParams (name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i"); 
            var r = window.location.search.substr(1).match(reg); 
            if (r != null) return unescape(r[2]); return null; 
        }

        
    },
    created () {
        let code = this.getUrlParams('code')
        console.log('code', code)
        let link = this.$route.query.link
        let consultNumber = this.$route.query.consultNumber
        if (link) {
            window.sessionStorage.setItem('link', link)
            window.sessionStorage.setItem('consultNumber', consultNumber)
            console.log('link', link)
        } 
         
        let wecharOpenID = window.localStorage.getItem('wecharOpenID1')
        console.log('loginWecharOpenID',wecharOpenID)
        if (!wecharOpenID || wecharOpenID === 'undefined' || wecharOpenID === 'null') {
            if (code) { // 调用获取用户信息接口
                this.getUserInfo(code)
            } 
            else { // 没有code调用授权链接
                console.log('获取code')
                this.getAppIdF()
            }
        } else {
            this.openIdGetUser()
        }
        
        
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.login {
    ul {
        padding-top: 20px;
    }
    h1 {
        text-align: center;
        line-height: 50px;
    }
}

</style>