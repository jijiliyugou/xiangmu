<template>
    <div class="version">
        <!-- 版本号：V01R01B80901 -->
        <!-- <mt-field label="openId" placeholder="请输入openId" v-model="wecharOpenID1"></mt-field>
        <div class="okCase">
            <mt-button @click="applyOpenId" readonly  type="primary" class="mint-button--large">提交</mt-button>
        </div> -->
    </div>
</template>

<script>
import { Toast } from 'mint-ui';
export default {
    data () {
        return {
            wecharOpenID1: '',
        }
    },
    created() {
        window.localStorage.setItem('wecharOpenID', '')
        window.localStorage.setItem('wecharOpenID1', '')
        window.sessionStorage.setItem('userToken', '')
        window.sessionStorage.setItem('userId', '')
        window.sessionStorage.setItem('mobileRoleName', '')
        Toast('缓存清理成功！')
    },
    methods: {
        applyOpenId () {
            window.localStorage.setItem('wecharOpenID1', this.wecharOpenID1)
            this.$router.push({ 
                path: '/index-patient'
            })
        }
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.version {
    background: $color-wfont;
    text-align: center;
    font-size: 24px;
    .okCase {
        padding: 20px 10px;
    }
    // padding: 50px 10px;
}
</style>