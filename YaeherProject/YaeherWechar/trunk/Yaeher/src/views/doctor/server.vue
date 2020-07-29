<template>
    <div class="server padding-top">
        <mt-header fixed title="我的服务">
            <a @click="$router.push({path: '/doctor-user'})" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
            <ul class="server-margin">
                <router-link v-for="(item, index) in userServerList" :key="index" tag="li" :to="{path: '/set-server', query: {type: item.serviceType, id: item.id, set: 'alter'}}" class="flex common-flex">
                    <div class="common-left">
                        <img src="../../assets/image/lu.png" />
                        <div>{{item.serviceTypeValue}}咨询</div>
                    </div>
                    <div class="common-right">
                        <span v-if="!item.serviceState">休息中</span>
                        <i class="mint-cell-allow-right"></i>
                    </div>
                </router-link>
                <router-link v-if="item.show" v-for="(item, index) in serverType" :key="index" tag="li" :to="{path: '/set-server', query: {code: item.code, set: 'add'}}" class="flex common-flex">
                    <div class="common-left">
                        <img src="../../assets/image/lu.png" />
                        <div>{{item.value}}咨询</div>
                    </div>
                    <div class="common-right">
                        未开通
                        <i class="mint-cell-allow-right"></i>
                    </div>
                </router-link>
                <!-- <router-link tag="li" to="/arrange" class="flex common-flex">
                    <div class="common-left">
                        <img src="../../assets/image/lu.png" />
                        <div>门诊排班</div>
                    </div>
                    <div class="common-right">
                        <i class="mint-cell-allow-right"></i>
                    </div>
                </router-link> -->
            </ul>
        </div>
    </div>
</template>

<script>
export default {
    data () {
        return {
            userServerList: [],
            serverType: [],
            ImgShow: true,
            phonShow: true
        }
    },
    methods: {
        getServerType() { // 获取服务类型
            this.instance.serviceMoneyListTypeD({
            })
                .then((response) => {
                    const _this = this
                    this.serverType = response.data.result.item
                    for (let i = 0; i < this.serverType.length; i++) {
                        if (this.serverType[i].code === 'ImageText') {
                            this.serverType[i].show = this.ImgShow 
                        } else if (this.serverType[i].code === 'Phone') {
                            this.serverType[i].show = this.phonShow 
                        }
                        console.log(this.serverType)
                    }
                    

                })
                .catch((error) => {
                }) 
        },
        getUserServer() { // 获取我的服务
            this.instance.serviceMoneyListD({
            })
                .then((response) => {
                    this.userServerList = response.data.result.item
                    console.log('1111', this.userServerList)
                    if (this.userServerList) {
                        for (let i = 0; i < this.userServerList.length; i++) {
                            if (this.userServerList[i].serviceType === 'ImageText') {
                                this.ImgShow = false
                            } else if (this.userServerList[i].serviceType === 'Phone') {
                                this.phonShow = false
                            }
                            
                        }
                    } else {
                        this.userServerList = []
                    }
                    this.getServerType()
                    
                })
                .catch((error) => {
                }) 
        }
    },
    mounted () {
        this.getUserServer()
    },
    created () {
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.server {
    .server-margin {
        margin-top: 10px;
        padding: 0 0 0 15px;
        .common-flex {
            .common-left {
                display: flex;
                img {
                    display: block;
                    height: 40px;
                    width: 40px;
                    border-radius: 20px;
                }
                div {
                    flex: 1;
                    padding: 0 0 0 10px;
                    line-height: 40px;
                }
            }
            .common-right {
                text-align: right;
                position: relative;
                line-height: 40px;
                padding-right: 35px;
            }
        }
    }
}


</style>