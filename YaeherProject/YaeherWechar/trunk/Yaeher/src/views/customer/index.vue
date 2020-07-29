<template>
    <div class="index-customer padding-top">
        <mt-header fixed title="待审核"></mt-header>
        <div class="content p-content listAbout">
            <div class="searchCase">
                <div class="searchBox">
                    <input @keypress="searchKeypress" ref="inputText" type="search" v-model="keyWord" placeholder="请输入医生名查找" />
                    <div @click="goSearch(true)">搜索</div>
                </div>
            </div>
            <ul v-infinite-scroll="goSearch"
  				infinite-scroll-disabled="loading"
  				infinite-scroll-distance="10" class="group2-list">
                <li v-for="(item, index) in doctorList" :key="index">
                    <router-link :to="{path: '/audit-doctor', query: {type: 'customer', id: item.id}}" class="doctor-a">
                        <div class="flex doctorInfo">
                            <img v-if="item.userImage != null" :src="item.userImage" :onerror="stallDefaultImg" align="avater" />
                            <img v-if="item.userImage === null" src="../../assets/image/logo.jpg" alt="" />
                            <div class="flexCase">
                                <i class="mint-cell-allow-right"></i>
                                <p class="doctor-name">
                                    {{item.doctorName}}
                                </p>
                                <p>{{item.department}} {{item.title}}</p>
                                <p><span>{{item.hospitalName}}</span></p>
                                <span v-if="item.authCheckRes ==='unupload'" class="doctor-hint red-hint">未上传</span>
                                <span v-if="item.authCheckRes ==='upload'" class="doctor-hint red-hint">已上传</span>
                                <span v-if="item.authCheckRes ==='success'" class="doctor-hint green-hint">已认证</span>
                                <span v-if="item.simTestRes ==='UnExam'" class="doctor-hint1 red-hint">未考试</span>
                                <span v-if="item.simTestRes ==='fail'" class="doctor-hint1 red-hint">考试未通过</span>
                                <span v-if="item.simTestRes ==='success'" class="doctor-hint1 green-hint">考试通过</span>
                                <span class="time-hint">{{item.createdOn}}</span>
                            </div>
                        </div>
                    </router-link>
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
import { Toast } from 'mint-ui';
let moment = require('moment');
export default {
    data () {
        return {
            selected: 'nav1',
            keyWord: '',
            doctorList: [],
            skipCount: 1,
			maxResultCount: 10,
			totalPage: 2,
            stallDefaultImg: 'this.src="' + require('assets/image/logo.jpg') + '"',
            loading: true
        }
    },
    methods: {
        searchKeypress(event) {
            if (event.keyCode == 13) {
                this.goSearch(true)
				//搜索
            } 
        },
        goSearch (flag) {
            this.loading = true
            if (flag) {
                this.skipCount = 1
                this.totalPage = 2
            }
            const keyWord = this.keyWord
            const maxResultCount  = this.maxResultCount 
			const skipCount = this.skipCount
			if (skipCount > this.totalPage) {
				if(skipCount > 2) {
                    Toast('没有更多了！')
                }
				return
			}
            this.$refs.inputText.blur() 
            this.instance.yaeherDoctorPageA({
                keyWord,
                maxResultCount,
                skipCount
            })
                .then((response) => {
                    setTimeout(() => {
                        this.loading = false
                        let moreFlag = response.data.result.item.items
                        if (!moreFlag || moreFlag.length===0) {
							this.loading = true
                        }
                        if (flag) this.doctorList = []
                        this.doctorList = this.doctorList.concat(response.data.result.item.items)
                        for (let i = 0; i < this.doctorList.length; i++) {
                            this.doctorList[i].createdOn = moment(this.doctorList[i].createdOn).format('YYYY-MM-DD HH:mm:ss')
                        }
                        this.skipCount ++
                        this.totalPage = response.data.result.item.totalPage
                        console.log(this.skipCount, this.totalPage)
                    }, 100);    
                })
                .catch((error) => {
                    this.loading = true
                }) 
        },
    },
    mounted () {
        this.goSearch()
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.index-customer {
    .listAbout {
        position: absolute; left: 0; right: 0; bottom: 0px;top: 42px;
    }
    .group2-list { 
        font-size: $font-m;
        color: $color-bfont;
        background: $color-wfont;
        li {
            padding: 15px 0; 
            display: flex;
            .doctor-a {
                display: block;
                width: 100%;
                .doctorInfo {
                    img{
                        display: block;
                        width: 60px;
                        height: 60px;
                        border-radius: 5px;
                    }
                    .flexCase {
                        padding: 0 10px; 
                        position: relative;
                        .doctor-hint {
                            position: absolute;
                            right: 5px;
                            top: -2px;
                        }
                        .doctor-hint1 {
                            position: absolute;
                            right: 5px;
                            top: 12px;
                        }
                        .time-hint {
                            position: absolute;
                            right: 5px;
                            bottom: 2px;
                        }
                        p {
                            font-size: $font-m;
                            color: $color-bfont;
                            padding-bottom: 2px;
                        }
                        .doctor-name {
                            font-size: $font-xl;
                            color: $color-afont;
                        }
                        .star-font {
                            color: $color-star;
                        }
                    }
                }  
            }
        }
    }
}


</style>