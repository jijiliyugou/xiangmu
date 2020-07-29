<template>
    <div class="information-list padding-top">
        <mt-header fixed title="我的消息">
        </mt-header>
        <div class="content p-content listAbout">
            <div v-if="infoCount > 0" @click="hintClick" class="hintInfor">未读消息{{infoCount}}条</div>
            <ul v-infinite-scroll="getInforList"
  				infinite-scroll-disabled="loading"
  				infinite-scroll-distance="10" class="informationList">
                <li v-for="(item, index) in userList" :key="index" :class="{grayLi: item.acceptState === '1'}">
                    <a @click="goDetail(item.id, item.nickName, item.consultantOpenID, item.acceptState)" class="doctor-a">
                        <div class="flex doctorInfo">
                            <!-- <span class="infoNub">10</span> -->
                            <img v-if="item.userImg != null" :src="item.userImg" />
                            <img v-if="item.userImg === null" src="../../assets/image/lu.png" />
                            <div class="flexCase">
                                <p class="doctor-name">
                                    {{item.nickName}}
                                </p>
                                <!-- <p class="simpleness"></p> -->
                                <span v-if="!item.isReady" class="doctor-hint red-hint">待处理</span>
                                <span class="imformation-time">{{item.acceptTime}}</span>
                            </div>
                        </div>
                    </a>
                </li>
            </ul>        
        </div>
        <mt-tabbar v-model="selected" fixed>
            <mt-tab-item id="nav1" href="#/index-customer">
                <img slot="icon" src="../../assets/image/question-icon.png">
                待审核
            </mt-tab-item>
            <mt-tab-item id="nav2" href="#/information-list">
                <img slot="icon" src="../../assets/image/class-icon.png">
                我的消息
            </mt-tab-item>
            <mt-tab-item id="nav3" href="#/customer-user">
                <img slot="icon" src="../../assets/image/user-icon.png">
                个人中心
            </mt-tab-item>
        </mt-tabbar>
    </div>
</template>

<script>
let moment = require('moment');
let interval
export default {
    data () {
        return {
            selected: 'nav2',
            userList: [],
            skipCount: 1,
            maxResultCount: 10,
            totalPage: 2,
            loading: true,
            infoCount: 0,
            startTime: '',
            consultantID: 0
        }
    },
    methods: {
        getInforList(flag) { // 获取用户列表
            if (flag) {
                this.skipCount = 1
                this.totalPage = 2
            }
            const maxResultCount  = this.maxResultCount 
			const skipCount = this.skipCount
			if (skipCount > this.totalPage) {
				if(skipCount > 2) {
                    Toast('没有更多了！')
                }
				return
			}
            this.instance.acceptWecharStatePage({
                skipCount,
                maxResultCount,
            })
                .then((response) => {
                    if (response.data.result.code === 200) {
                        this.infoCount = 0
                        this.startTime = moment().format('YYYY-MM-DD HH:mm:ss')
                        setTimeout(() => {
                            let moreFlag = response.data.result.item.items
                            if (!moreFlag || moreFlag.length===0) {
                                this.loading = true
                            }
                            // this.userList = response.data.result.item
                            if (flag) this.userList = []
                            this.userList = this.userList.concat(response.data.result.item.items)
                            for (let i = 0; i < this.userList.length; i++) {
                                this.userList[i].acceptTime = moment(this.userList[i].acceptTime).format('YYYY-MM-DD HH:mm:ss')
                            }
                            this.loading = false
                            this.skipCount ++
                            this.totalPage = response.data.result.item.totalPage
                        }, 100);     
                    }
                    
                })
                .catch((error) => {
                    this.loading = true
                }) 
            
        },
        getCount() { // 获取消息条数
            let startTime = this.startTime
            this.instance.unReadyWecharList({
                startTime
            })
                .then((response) => {
                    if (response.data.result.code === 200) {  
                        this.infoCount = response.data.result.item
                    }
                    
                })
                .catch((error) => {
                }) 
            
        },
        goDetail (id1, consultantName1, fromUserName1, acceptState1) {
            const id = id1
            const consultantName = consultantName1
            const fromUserName = fromUserName1
            const acceptState = acceptState1
            this.$router.push({ 
                path: '/information-detail',
                query: {
                    id, 
                    consultantName, 
                    fromUserName, 
                    acceptState
                }
            })
        },
        addInfo() {
            let consultantID = this.consultantID
            this.instance.joinWecharStateC({
                consultantID
            })
                .then((response) => {
                    if (response.data.result.code === 200) {  
                        this.getInforList()
                    }
                    
                })
                .catch((error) => {
                }) 
        },
        hintClick() {
            this.getInforList(true)
        },
        
    },
    beforeDestroy() {
		clearInterval(interval);
	},
    mounted () {
        if (this.consultantID === 0) {
            this.getInforList()
        } else {
            this.addInfo()
        }
        this.getCount()
        let _this = this
		interval = window.setInterval(function() {
				_this.getCount()
            }, 30000)
    },
    created() {
        this.startTime = moment().format('YYYY-MM-DD HH:mm:ss')
        let consultantID1 = this.$route.query.consultantID
        if (consultantID1) {
            this.consultantID = parseInt(consultantID1)
        }
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.information-list {
    -webkit-user-select: text;
    -moz-user-select: text;
    -ms-user-select: text;
    user-select: text;
    .listAbout {
        position: absolute; left: 0; right: 0; bottom: 0px;top: 42px;
    }
    .hintInfor {
        padding: 8px;
        text-align: center;
        background-color: #e8edfe;

    }
    .informationList { 
        padding: 0px;
        font-size: $font-m;
        color: $color-bfont;
        background: $color-wfont;
        li {
            padding: 15px 10px; 
            display: flex;
            .doctor-a {
                display: block;
                width: 100%;
                .doctorInfo {
                    position: relative;
                    .infoNub {
                        background: #FF0000;
                        color: #fff;
                        font-size: 10px;
                        line-height: 11px;
                        display: inline-block;
                        padding: 3px;
                        border-radius: 8px;
                        position: absolute;
                        top: -5px;
                        left: 40px;
                    }
                    img{
                        display: block;
                        width: 50px;
                        height: 50px;
                        border-radius: 5px;
                    }
                    .flexCase {
                        padding: 0 10px; 
                        position: relative;
                        .doctor-hint {
                            position: absolute;
                            right: 5px;
                            top: 15px;
                        }
                        .imformation-time {
                            position: absolute;
                            right: 5px;
                            top: 0;
                            color: $color-cfont;
                        }
                        p {
                            font-size: $font-m;
                            color: $color-bfont;
                            padding-bottom: 2px;
                        }
                        .doctor-name {
                            font-size: $font-xl;
                            color: $color-afont;
                            padding-top: 4px;
                        }
                        .simpleness {
                            width: 240px;
                            font-size: $font-m;
                            color: $color-bfont;
                            overflow: hidden;
                            white-space: nowrap;
                            text-overflow: ellipsis;
                        }
                    }
                }  
            }
        }
        .grayLi {
            background: #ccc;
        }
    }
}

</style>