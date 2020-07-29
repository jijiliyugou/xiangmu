<template>
    <div class="order-list-control padding-top">
		<mt-header fixed title="咨询列表">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content listAbout">
			<div class="searchCase">
                <div class="searchBox">
                    <input @keypress="searchKeypress" ref="inputText" type="search" v-model="keyWord" placeholder="请输入关键字查找" />
                    <div @click="getOrderList(true)">搜索</div>
                </div>
            </div>
            <ul v-infinite-scroll="getOrderList"
  				infinite-scroll-disabled="loading"
  				infinite-scroll-distance="10"  class="mui-table-view recordList">
				<li v-for="(item, index) in recordList" :key="index" class="mui-table-view-cell">
			        <router-link :to="{path: '/order-control', query: {id: item.id}}" class="mui-navigate-right">
			        	<div class="recordCase">
			        		<div class="recordTop">
			        			<div class="recordLeft">
				        			<div class="recordTitle">
                                        <span class="serverType" v-if="item.consultType === 'ImageText'">文</span>
										<span class="serverType" v-if="item.consultType === 'Phone'">电</span>
                                        <p class="font-gray"><span>{{item.doctorName}}</span></p>
				        				<p class="font-gray"> 患者：{{item.patientName}} {{item.sex | sexSelect }} {{item.age}} {{item.iiInessType}}</p>
				        				<p class="font-overflow">{{item.iiInessDescription}}</p>
				        			</div>
			        			</div>
								<div class="recordRight">
                                    <star :star="item.evaluateLevel"></star>
                                    <!-- <p class="hint green-hint">已完成</p> -->
			        			</div>
			        		</div>
			        		<div class="recordBottom">
			        			<div class="userTime">
			        				<p>{{item.createdOn}}</p>
			        			</div>
			        		</div>
			        	</div>
			        </router-link>
			    </li>
            </ul>  
        </div>
    </div>
</template>

<script>
import { Tabbar, TabItem, Toast } from 'mint-ui';
import { createSecret } from 'assets/js/common.js'
import Star from 'components/star/star'
let moment = require('moment');
export default {
    components: {
      Star
    },
    data () {
        return {
            keyWord: '',
            id: 0,
            title: '',
            recordList: [],
            skipCount: 1,
			maxResultCount: 10,
            totalPage: 2,
            loading: true
        }
    },
    filters: {
        sexSelect (value) {
            const sexStatus = ['', '男', '女']
            return sexStatus[value]
        }
    },
	methods: {
        getOrderList(flag) { // 获取申诉列表
            this.loading = true
            if (flag) {
                this.skipCount = 1
                this.totalPage = 2
            }
            const keyWord = this.keyWord
            const doctorID = this.id
            const maxResultCount  = this.maxResultCount 
            const skipCount = this.skipCount
            
			if (skipCount > this.totalPage) {
				if(skipCount > 2) {
                    Toast('没有更多了！')
                }
				return
			}
            this.$refs.inputText.blur()
            this.instance.consultationPageD({
                doctorID,
                keyWord,
                skipCount,
                maxResultCount
            })
                .then((response) => {
                    console.log(2222)
                    if (response.data.result.code === 200) {
                        setTimeout(() => {
                            this.loading = false
                            let moreFlag = response.data.result.item.items
                            
                            if (!moreFlag || moreFlag.length===0) {
                                this.loading = true
                            }
                            if (flag) this.recordList = []
                            this.recordList = this.recordList.concat(response.data.result.item.items)
                            this.skipCount ++
                            this.totalPage = response.data.result.item.totalPage
                            console.log(this.skipCount, this.totalPage)
                            for (let i = 0; i < this.recordList.length; i++) {
                                this.recordList[i].createdOn = moment(this.recordList[i].createdOn).format('YYYY-MM-DD HH:mm:ss')
                            }
                        }, 100);   
                    } else {
                        this.loading = true
                    }
                     
                })
                .catch((error) => {
                    console.log(1111111111)
                    this.loading = true
                }) 
        },
        searchKeypress (event) {
            if (event.keyCode == 13) {
                this.getOrderList(true)
            } 
        }
    },
	mounted () {
		this.getOrderList()
	},
    created () {
        this.id = parseInt(this.$route.query.id)
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.order-list-control{
    .listAbout {
        position: absolute; left: 0; right: 0; bottom: 0px;top: 42px;
    }
	.recordList {
        margin: 10px 0 0;
        .recordCase {
            padding: 0px 10px; 
            font-size: $font-m;
            .recordTop {
                display: flex; 
                padding-bottom: 0px;
                .recordLeft {
                    flex: 1; 
                    display: flex;
                    .recordTitle {
                        flex: 1; 
                        text-align: left;
                        position: relative;
                        .serverType {
                            width: 18px; 
                            height: 18px; 
                            color: $color-wfont; 
                            background-color: $color-red; 
                            position: absolute; 
                            top:2px; 
                            left: -21px; 
                            font-size: $font-m;
                            display: inline-block; 
                            border-radius: 9px; 
                            line-height: 19px; 
                            text-align: center;
                        }
                        .font-gray {
                            color: $color-bfont;
                            span {
                                font-size: $font-l; 
                                color: $color-afont;
                            }
                        }
                        .font-overflow { 
                            width: 240px; 
                            margin: 5px 0; 
                            font-size: $font-l; 
                            overflow: hidden; 
                            text-overflow:ellipsis; 
                            word-break: break-all;
                            white-space: nowrap;
                        }
                    }
                }
                .recordRight {
                    width: 90px; 
                    color: $color-green; 
                    text-align: right;
                    .hint {
                        padding-top: 10px;
                        font-size: $font-l;
                    }
                }
            }
            .recordBottom {
                padding-top: 1px;
                p {
                    font-size: $font-m;
                    color: $color-bfont;
                }
            }
        }
    }
}
</style>