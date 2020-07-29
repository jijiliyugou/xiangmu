<template>
    <div class="doctor-info-list padding-top">
        <mt-header fixed title="个人信息">
            <a @click="$router.push({ path: '/doctor-user'})" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="p-content">
            <router-link tag="div" to="/avatar" class="doctor-top flex">
                <div class="flex-left info-list-name">
                    <p>{{userdetail.doctorName}}</p>
                </div>
                <div class="flex-right">
                    <img :src="userdetail.userImage" />
                    <i class="mint-cell-allow-right"></i>
                </div>
            </router-link>
            <ul class="user-list doctor-user-list">
                <!-- <li>
                    <router-link to="/qrCode" class="user-href">
                        <div class="flex">
                            <div class="user-title">怡禾账号</div>
                            <div class="href-arrow right-paddding">
                                13011112222
                            </div>
                        </div>
                    </router-link>
                </li> -->
                <li>
                    <a class="user-href">
                        <div class="flex">
                            <div class="user-title">怡禾认证</div>
                            <router-link tag="div"  to="/agreement-login" v-if="userdetail.authCheckResCode === 'unupload'" class="href-arrow red-hint right-paddding">
                                未认证
                                <i class="mint-cell-allow-right"></i>
                            </router-link>
                            <router-link tag="div"  to="/agreement-login" v-if="userdetail.authCheckResCode === 'fail'" class="href-arrow red-hint right-paddding">
                                未通过
                                <i class="mint-cell-allow-right"></i>
                            </router-link>
                            <div @click="underReview" v-if="userdetail.authCheckResCode === 'upload'" class="href-arrow red-hint right-paddding">
                                审核中
                                <i class="mint-cell-allow-right"></i>
                            </div>
                            <div v-if="userdetail.authCheckResCode === 'success'" class="href-arrow green-hint right-paddding">
                                已认证
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </a>
                </li>
                <li>
                    <router-link to="/basic-info" class="user-href">
                        <div class="flex">
                            <div class="user-title">基本资料</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
                <li>
                    <router-link :to="{path: '/doctor-phone', query: {phoneNumber: userdetail.phoneNumber}}" class="user-href">
                        <div class="flex">
                            <div class="user-title">电话维护</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
                <li>
                    <router-link to="/introduce" class="user-href">
                        <div class="flex">
                            <div class="user-title">简介</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
                <li>
                    <router-link to="/disease" class="user-href">
                        <div class="flex">
                            <div class="user-title">病种</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
                <li>
                    <router-link to="/experience" class="user-href">
                        <div class="flex">
                            <div class="user-title">执业经历</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
                <li>
                    <router-link :to="{path: '/look-detail', query: {id}}" class="user-href">
                        <div class="flex">
                            <div class="user-title">预览主页</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
            </ul>    
        </div>
    </div>
</template>

<script>
import { Toast } from 'mint-ui';
export default {
    data () {
        return {
            id: 0,
            userdetail: {}
        }
    },
    methods: {
        getUserInfo() { // 请求个人信息
            this.instance.yaeherDoctorD({
            })
                .then((response) => {
                    this.userdetail = response.data.result.item
                    const userId = this.userdetail.id
                    this.id = userId
                    window.sessionStorage.setItem('userId', userId)
                })
                .catch((error) => {
                }) 
        },
        underReview () {
            Toast('您的信息正在认证中，请耐心等候！')
        }
    },
    mounted () {
        this.getUserInfo()
    },
    created () {
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.doctor-info-list {
    .doctor-top {background: $color-wfont; padding: 10px 15px;}
    .flex-right {flex: 1; text-align: right; padding-right: 15px; position: relative;}
    .right-paddding {text-align: right; padding-right: 30px;}
    .flex-right .mint-cell-allow-right::after {right: 0;}
    .info-list-name p {line-height: 41px;}
    .doctor-hint {font-size: $font-l; color: $color-red}
    .flex-right img{width: 40px; height: 40px; border-radius: 5px;}
    .doctor-user-list .user-title {padding-left: 0;}
    .doctor-user-list {margin-bottom: 10px;}
}

</style>