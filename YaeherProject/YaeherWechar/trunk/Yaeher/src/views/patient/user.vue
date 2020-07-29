<template>
    <div class="patient-user">
        <div class="content">
            <div class="userAvatar">
				<div class="userInfo">
                    <img v-if="userdetail.userImage != null" :src="userdetail.userImage" />
                    <img v-if="userdetail.userImage === null" src="../../assets/image/logo.jpg" alt="" />
					<!-- <p>{{userdetail.wecharName}}</p> -->
				</div>
			</div>
            <ul class="user-list">
                <li>
                    <router-link to="/user-record" class="user-href">
                        <img class="user-icon user-icon1" src="../../assets/image/question-icon.png" />
                        <div class="flex">
                            <div class="user-title">我的咨询</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
                <li>
                    <router-link to="/patient-info" class="user-href">
                        <img class="user-icon user-icon1" src="../../assets/image/user-icon.png" />
                        <div class="flex">
                            <div class="user-title">个人信息</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
                <li>
                    <a href="https://m.qlchat.com/wechat/page/live/210000155031160" class="user-href">
                        <img class="user-icon user-icon1" src="../../assets/image/class-icon.png" />
                        <div class="flex">
                            <div class="user-title">我的课程</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </a>
                </li>
                <li>
                    <router-link to="/user-doctor" class="user-href">
                        <img class="user-icon user-icon2" src="../../assets/image/doctor-icon.png" />
                        <div class="flex">
                            <div class="user-title">我的医生</div>
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
export default {
    data () {
        return {
            userdetail: {},
        }
    },
    methods: {
        getUserInfo() { // 请求个人信息
            this.instance.patientInfo({
            })
                .then((response) => {
                    this.userdetail = response.data.result.item
                    const userId = response.data.result.item.id
                    window.sessionStorage.setItem('userId', userId)

                })
                .catch((error) => {
                }) 
        }
    },
    mounted () {
        
    },
    created () {
        this.getUserInfo()
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.patient-user {
    .userAvatar {width: 100%; height: 180px; display: flex; justify-content: center; align-items: center; background: url(../../assets/image/patientBg.jpg) no-repeat;}
    .userInfo { width: 200px; text-align: center;}
    .userInfo img {width: 80px; height: 80px; border-radius: 40px;}
    .userInfo p {font-size: $font-l; margin-bottom: 0; color: #999;}
    .user-href {position: relative;}
    .user-icon {position: absolute; width: 20px; height: 20px; top: 10px; left: 5px;}
    .user-icon1 {top: 11px;}
    .user-icon2 {top: 9px;}
}



</style>