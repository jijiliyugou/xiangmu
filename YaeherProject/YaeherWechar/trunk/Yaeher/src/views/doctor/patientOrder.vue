<template>
    <div class="patient-order padding-top">
		<mt-header fixed title="咨询记录">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>                
        </mt-header>
        <div class="content listAbout">
			<div class="searchCase">
                <div class="searchBox">
                    <input @keypress="searchKeypress" ref="inputText" type="text" v-model="keyWord" placeholder="请输入关键字" />
                    <div @click="goSearch">搜索</div>
                </div>
            </div>
            <ul v-infinite-scroll="getOrderList"
  				infinite-scroll-disabled="loading"
  				infinite-scroll-distance="10" 
				class="mui-table-view recordList">
				<li v-for="(item, index) in recordList" :key="index" class="mui-table-view-cell">
			        <router-link :to="{path: '/order-look', query: {id: item.id}}" class="mui-navigate-right">
			        	<div class="recordCase">
			        		<div class="recordTop">
			        			<div class="recordLeft">
				        			<div class="recordTitle">
										<span v-if="item.consultType === 'ImageText'">文</span>
										<span v-if="item.consultType === 'Phone'">电</span>
				        				<p class="font-gray">{{item.patientName}} {{item.sex | sexSelect}} {{item.age}} {{item.iiInessType}}</p>
				        				<p class="font-overflow">{{item.iiInessDescription}}</p>
				        			</div>
			        			</div>
			        			<div class="recordRight">
									<div v-if="item.consultStateCode!=statusSuccess" class="red-hint hint">{{item.consultState}}</div>
									<!-- <div v-if="item.consultStateCode===statusSuccess && !item.isReturnVisit"  class="red-hint hint">未回访</div>
									<div v-if="item.consultStateCode===statusSuccess && item.isReturnVisit"  class="green-hint hint">已回访</div> -->
									<div v-if="item.consultStateCode===statusSuccess && !item.isEvaluate"  class="red-hint hint">未评价</div>
									<div v-if="item.consultStateCode===statusSuccess && item.isEvaluate"  class="green-hint hint">已评价</div>
			        			</div>
			        			
			        		</div>
			        		<div class="recordBottom">
			        			<div class="userTime">
			        				<p>剩余{{item.hasInquiryTimes}}次追问</p>
			        				<p class="text-right">{{item.createdOn}}</p>
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
import { Toast } from 'mint-ui';
let moment = require('moment');
export default {
    data () {
        return {
			createdBy: 0,
            keyWord: '',
			activeShow: true,
			recordList: [],
			hasReply: 1,
			statusSuccess: 'success',
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
		searchKeypress(event) {
            if (event.keyCode == 13) {
				this.goSearch()
            } 
		},
		goSearch () {
			this.getOrderList(true)
			// const keyWord = this.keyWord
			// const createdBy = this.createdBy
			// if (!keyWord) {
			// 	Toast('请输入搜索内容')
			// 	return
			// }
			// this.$router.push({ 
			// 	path: '/doctor-search',
			// 	query: {
			// 		createdBy,
			// 		keyWord
			// 	}
			// })
		},
		getOrderList(flag) { // 获取咨询列表
			this.loading = true
			if (flag) {
                this.skipCount = 1
                this.totalPage = 2
            }
			const createdBy = this.createdBy
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
            this.instance.consultationPageD({
				keyWord,
				createdBy,
				skipCount,
				maxResultCount
            })
                .then((response) => {
					setTimeout(() => {
						this.loading = false
						let moreFlag = response.data.result.item.items
                        if (!moreFlag || moreFlag.length===0) {
							this.loading = true
						}
						if (flag) this.recordList = []
						if (response.data.result.item.items) {
							this.recordList = this.recordList.concat(response.data.result.item.items)
							for (let i = 0; i < this.recordList.length; i++) {
								this.recordList[i].createdOn = moment(this.recordList[i].createdOn).format('YYYY-MM-DD HH:mm:ss')
							}
							this.skipCount ++
							this.totalPage = response.data.result.item.totalPage
							console.log(this.skipCount, this.totalPage)
						}
					}, 100);
                })
                .catch((error) => {
                }) 
		}
	},
	mounted () {
		this.getOrderList()
	},
	created () {
		this.createdBy = parseInt(this.$route.query.createdBy)
	}
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.patient-order {
	.listAbout {
		position: absolute;
		left: 0;
		right: 0;
		bottom: 0px;
		top: 42px;
	}
	.recordCase {padding: 0px 10px; font-size: $font-m;}
	.recordList {margin: 10px 0 0;}
	.recordList .mui-table-view-cell:after {height: 5px;}
	.recordTop {display: flex; padding-bottom: 0px;}
	.font-gray {color: $color-bfont;}
	.recordRight {width: 50px; color: $color-green; text-align: right;}
	.recordRight .hint {padding-bottom: 3px;}
	.recordLeft {flex: 1; display: flex;}
	.recordAvatar {width: 70px;}
	.recordTitle {flex: 1; text-align: left; position: relative;}
	.recordTitle span { width: 18px; height: 18px; color: $color-wfont; background-color: $color-red; position: absolute; top:-2px; left: -21px; font-size: $font-m;
	display: inline-block; border-radius: 9px; line-height: 19px; text-align: center;}
	.recordBottom {padding-top: 1px;}
	.userTime {display: flex;}
	.userTime p {flex: 1; font-size: $font-m; color: $color-bfont}

	.font-overflow { 
		width: 250px; 
		margin: 5px 0; 
		font-size: $font-l; 
		white-space: nowrap; 
		overflow: hidden; 
		text-overflow: ellipsis;
	}
}

</style>