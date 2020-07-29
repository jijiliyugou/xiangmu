<template>
    <div class="doctor-user padding-top">
        <mt-header fixed title="个人中心">
        </mt-header>
        <div class="p-content">
            <router-link :to="{path: 'doctor-info-list'}">
                <div class="doctor-top flex">
                    <div class="flex-left">
                        <p>{{userdetail.doctorName}}</p>
                        <p v-if="userdetail.authCheckResCode === 'unupload'" class="doctor-hint red-hint">未认证</p>
                        <p v-if="userdetail.authCheckResCode === 'upload'" class="doctor-hint red-hint">审核中</p>
                        <p v-if="userdetail.authCheckResCode === 'fail'" class="doctor-hint red-hint">不通过</p>
                        <p v-if="userdetail.authCheckResCode === 'success'" class="doctor-hint green-hint">已认证</p>
                    </div>
                    <div class="flex-right">
                        <img :src="userdetail.userImage" />
                        <i class="mint-cell-allow-right"></i>
                    </div>
                </div>
            </router-link>
            <ul class="user-list doctor-user-list">
                <li>
                    <router-link :to="{path: '/qrCode', query: {hospitalName: userdetail.hospitalName}}" class="user-href">
                        <div class="flex">
                            <div class="user-title">二维码</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
                <li v-if="userdetail.authCheckResCode === 'success'">
                    <router-link to="/server" class="user-href">
                        <div class="flex">
                            <div class="user-title">我的服务</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
                <li v-if="userdetail.authCheckResCode != 'success'" @click="msgHint('您的认证还未成功,不能进行服务设置')">
                    <a class="user-href">
                        <div class="flex">
                            <div class="user-title">我的服务</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </a>
                </li>
                <!-- <li>
                    <router-link to="/payment-term" class="user-href">
                        <div class="flex">
                            <div class="user-title">我的收款方式</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li> -->
                <!-- <li>
                    <router-link to="/index-doctor" class="user-href">
                        <div class="flex">
                            <div class="user-title">历史咨询</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li> -->
                <li>
                    <router-link to="/ranking" class="user-href">
                        <div class="flex">
                            <div class="user-title">我的收入</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
                <!-- <li>
                    <router-link to="/ranking" class="user-href">
                        <div class="flex">
                            <div class="user-title">我的排行</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li> -->
                <li>
                    <router-link to="/evaluate-doctor" class="user-href">
                        <div class="flex">
                            <div class="user-title">我的评价</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
                <li>
                    <router-link to="/illness-case" class="user-href">
                        <div class="flex">
                            <div class="user-title">我的病例夹</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
                <li>
                    <router-link to="/doctor-article" class="user-href">
                        <div class="flex">
                            <div class="user-title">我的文章</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
                <li v-if="userdetail.authCheckResCode === 'success'">
                    <router-link to="/administrative-office" class="user-href">
                        <div class="flex">
                            <div class="user-title">科室管理</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
                <li v-if="userdetail.authCheckResCode != 'success'" @click="msgHint('您的认证还未成功,不能进行科室管理')">
                    <a class="user-href">
                        <div class="flex">
                            <div class="user-title">科室管理</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </a>
                </li>
                <li v-if="userdetail.authCheckResCode != 'success'" @click="msgHint('您的认证还未成功,不能进行质控委员申请')">
                    <a class="user-href">
                        <div class="flex">
                            <div class="user-title">质控委员</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </a>
                </li>
                <li v-if="userdetail.authCheckResCode === 'success'">
                    <router-link :to="{path:'/apply-control', query: {isQuality: nub}}" class="user-href">
                        <div class="flex">
                            <div class="user-title">质控委员</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
                <li v-if="isQuality">
                    <router-link to="/audit-process" class="user-href">
                        <div class="flex">
                            <div class="user-title">申诉审核处理</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
            </ul>    
        </div>
        <mt-tabbar v-model="selected" fixed>
            <mt-tab-item id="nav1" href="#/index-doctor">
                <img slot="icon" src="../../assets/image/question-icon.png">
                咨询
            </mt-tab-item>
            <mt-tab-item id="nav2" href="#/guide">
                <img slot="icon" src="../../assets/image/class-icon.png">
                指南
            </mt-tab-item>
            <mt-tab-item id="nav3" href="#/doctor-user">
                <img slot="icon" src="../../assets/image/user-icon.png">
                我的
            </mt-tab-item>
        </mt-tabbar>
    </div>
</template>

<script>
import { Toast } from 'mint-ui'
export default {
    data () {
        return {
            selected: 'nav3',
            id: 0,
            userdetail: {},
            isQuality: false,
            nub: ''
        }
    },
    methods: {
        msgHint(msg) {
            Toast(msg)
        },
        getUserInfo() { // 请求个人信息
            this.instance.yaeherDoctorD({
            })
                .then((response) => {
                    this.userdetail = response.data.result.item
                    const userId = response.data.result.item.id
                    const doctorName = response.data.result.item.doctorName
                    this.id = userId
                    this.isQuality = this.userdetail.isQuality
                    if (this.isQuality) {
                        this.nub = 'yes'
                    } else {
                        this.nub = 'no'
                    }
                    window.sessionStorage.setItem('userId', userId)
                    window.sessionStorage.setItem('userName', doctorName)
                    window.sessionStorage.setItem('userImage', this.userdetail.userImage)
                })
                .catch((error) => {
                }) 
        },
        replyParameter () { // 输入框限制参数
            this.instance.consultationReplyParameter({
            })
                .then((response) => {
					let replyList = response.data.result.item
                    const maxReplyLength = replyList.maxReplyLength
                    window.sessionStorage.setItem('maxReplyLength', maxReplyLength)
                })
                .catch((error) => {
                }) 
		}
    },
    mounted () {
        this.getUserInfo()
        this.replyParameter()
    },
    created () {
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.doctor-user {
    .doctor-top {background: $color-wfont; padding: 15px;}
    .doctor-info-list {display: block;}
    .flex-right {flex: 1; text-align: right; padding-right: 15px; position: relative;}
    .flex-right .mint-cell-allow-right::after {right: 0;}
    .doctor-hint {font-size: $font-l;}
    .flex-right img{width: 40px; height: 40px; border-radius: 5px;}
    .doctor-user-list .user-title {padding-left: 0;}
    .doctor-user-list {margin-bottom: 10px;}
}

</style>