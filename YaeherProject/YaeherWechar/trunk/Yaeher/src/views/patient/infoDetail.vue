<template>
    <div class="patient-info-detail padding-top">
        <mt-header fixed title="个人信息">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
            <mt-button  slot="right">
                <a @click='saveInfo' class="right-white">保存</a>
            </mt-button> 
        </mt-header>
        <div class="content">
            <ul class="user-list">
                <mt-field label="用户名" placeholder="请输入用户名" type="text" v-model="loginName"></mt-field>
                <mt-field label="手机号" placeholder="请输入手机号" type="tel" v-model="phoneNumber"></mt-field>
            </ul>    
            
        </div>
    </div>
</template>

<script>
import { Toast } from 'mint-ui';
import { fontVery } from 'assets/js/common.js'
export default {
    data () {
        return {
            id: 0,
            loginName: '',
            phoneNumber: '',
        }
    },
    methods: {
        saveInfo () {
            const id = this.id
            const phoneNumber = this.phoneNumber
            const loginName = this.loginName
            if (!loginName) {
                Toast('用户名不能为空')
                return
            }
            if (!phoneNumber) {
                Toast('电话号码不能为空')
                return
            }
            // let fontFlag = fontVery(loginName)
            // if (!fontFlag) return
            this.instance.updateYaeherUser({
                id,
                phoneNumber,
                loginName
            })
                .then((response) => {
                    Toast('修改成功')
                    this.$router.push({path: '/patient-info'})
                })
                .catch((error) => {
                }) 
        },
        getUserInfo() { // 请求个人信息
            this.instance.patientInfo({
            })
                .then((response) => {
                    const infoDetail = response.data.result.item
                    this.loginName = infoDetail.loginName
                    this.phoneNumber = infoDetail.phoneNumber
                })
                .catch((error) => {
                }) 
        }
    },
    mounted () {
        this.getUserInfo()
        this.id = window.sessionStorage.getItem('userId')
    },
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

</style>