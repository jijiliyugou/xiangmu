<template>
    <div class="admin-doctor padding-top">
        <mt-header fixed title="">
            <!-- <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a> -->
            <mt-button @click="selectType(true, 1)" :class="{ 'activeCheck': activeShow }" slot="left">待处理</mt-button>
			<mt-button @click="selectType(false, 2)" :class="{ 'activeCheck': !activeShow }" slot="right">已处理</mt-button>
        </mt-header>
        <div class="content listAbout">
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
                    <router-link :to="{path: '/audit-doctor', query: {type: 'admin', id: item.id}}" class="doctor-a">
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
                                <span v-if="item.checkRes ==='checking'" class="doctor-hint red-hint">待审核</span>
                                <span v-if="item.checkRes ==='fail'" class="doctor-hint red-hint">不通过</span>
                                <span v-if="item.checkRes ==='success'" class="doctor-hint green-hint">已通过</span>
                                <span class="time-hint">{{item.createdOn}}</span>
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
let moment = require('moment');
export default {
    data () {
        return {
            keyWord: '',
            doctorList: [],
            skipCount: 1,
			maxResultCount: 10,
			totalPage: 2,
            stallDefaultImg: 'this.src="' + require('assets/image/logo.jpg') + '"',
            loading: true,
            activeShow: true,
            checkRes: 'checking'
        }
    },
    methods: {
        selectType(flag, index) {
			// const hasIndex = this.hasReply
            // if (hasIndex === index) return
            if (flag) {
                this.checkRes = 'checking'
            } else {
                this.checkRes = ''
            }
			this.skipCount = 1
			this.totalPage = 2
			this.loading = true
			this.doctorList = []
			this.activeShow = flag
			// this.hasReply = index
			this.goSearch(true)
		},
        searchKeypress(event) {
            if (event.keyCode == 13) {
                this.goSearch(true)
				//搜索
            } 
        },
        goSearch (flag) {
            this.loading = true
            let _this = this
            if (flag) {
                this.skipCount = 1
                this.totalPage = 2
            }
            const keyWord = this.keyWord
            const maxResultCount  = this.maxResultCount 
            const skipCount = this.skipCount
            const checkRes = this.checkRes 
			if (skipCount > this.totalPage) {
                if(skipCount > 2) {
                    Toast('没有更多了！')
                }
				return
			}
            this.$refs.inputText.blur()
            this.instance.yaeherDoctorPageA({
                keyWord,
                checkRes,
                skipCount,
                maxResultCount
            })
                .then((response) => {
                    
                    setTimeout(() => {
                        this.loading = false
                        if (flag) this.doctorList = []

                        let moreFlag = response.data.result.item.items
                        if (!moreFlag || moreFlag.length===0) {
							this.loading = true
                        }
                        
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

.admin-doctor {
    .listAbout {
        position: absolute; left: 0; right: 0; bottom: 0px;top: 42px;
    }
    .mint-header-title {display: none;}
	.mint-header-button {text-align: center;}
    .activeCheck {color: $color-star;}
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
                            top: 0;
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