<template>
    <div class="index-doctor">
		<mt-header fixed title="">
			<mt-button @click="selectType(true, 1)" :class="{ 'activeCheck': activeShow }" slot="left">待处理（{{totalCount}}）</mt-button>
			<mt-button @click="selectType(false, 2)" :class="{ 'activeCheck': !activeShow }" slot="right">已处理</mt-button>
		</mt-header>
        <div ref="srcollBox" class="listAbout">
			<!-- <div class="searchCase">
                <div class="searchBox">
                    <input @keypress="searchKeypress" type="text" v-model="iIInessDescription" placeholder="请输入关键字" />
                    <div @click="goSearch">搜索</div>
                </div>
            </div> -->
			<mt-loadmore v-show="listShow" :bottom-method="loadBottomUse1"
				:bottom-all-loaded="allUseLoad1" :bottomPullText='bottomText' :auto-fill="false"
				ref="loadmore1">
				<ul class="mui-table-view recordList">
					<li v-for="(item, index) in recordList1" :key="index" class="mui-table-view-cell">
						<router-link :to="{path: '/order-detail', query: {id: item.id}}" class="mui-navigate-right">
							<div class="recordCase">
								<div class="recordTop">
									<div class="recordLeft">
										<div class="recordTitle">
											<span class="imgTxt" v-if="item.consultType === 'ImageText'"></span>
											<span class="imgPhone" v-if="item.consultType === 'Phone'"></span>
											<p class="font-gray">{{item.patientName}} {{item.sex | sexSelect}} {{item.age}} {{item.iiInessType}}</p>
											<p class="font-overflow">{{item.iiInessDescription}}</p>
										</div>
									</div>
									<div class="recordRight">
										<!-- <div class="red-hint">超时</div> -->
										<div v-if="item.consultStateCode!=statusSuccess" class="red-hint hint">{{item.consultState}}</div>
										<!-- <div v-if="item.consultStateCode===statusSuccess && !item.isReturnVisit"  class="red-hint hint">未回访</div>
										<div v-if="item.consultStateCode===statusSuccess && item.isReturnVisit"  class="green-hint hint">已回访</div> -->
										<div v-if="item.consultStateCode===statusSuccess && !item.isEvaluate"  class="red-hint hint">未评价</div>
										<div v-if="item.consultStateCode===statusSuccess && item.isEvaluate"  class="green-hint hint">已评价</div>
										<!-- <div class="record-icon">
											<img v-if="item.consultType === 'ImageText'" src="../../assets/image/chat-icon.png" />
											<img v-if="item.consultType === 'Phone'" src="../../assets/image/phone-icon2.png" />
										</div> -->
									</div>
									
								</div>
								<div class="recordBottom">
									<div class="userTime">
										<p v-if="item.consultType != 'Phone'">剩余{{item.hasInquiryTimes}}次追问</p>
										<p v-if="activeShow && item.createdOnFormatter!=''" class="text-right">已等待{{item.createdOnFormatter}}</p>
										<p v-if="!activeShow && item.createdOnFormatter!=''" class="text-right">距离结束剩余{{item.createdOnFormatter}}</p>
									</div>
								</div>
							</div>
						</router-link>
					</li>
				</ul>  
			</mt-loadmore>
			<mt-loadmore v-show="!listShow" :bottom-method="loadBottomUse"
				:bottom-all-loaded="allUseLoad" :bottomPullText='bottomText' :auto-fill="false"
				ref="loadmore">
				<ul class="mui-table-view recordList">
					<li v-for="(item, index) in recordList" :key="index" class="mui-table-view-cell">
						<router-link :to="{path: '/order-detail', query: {id: item.id}}" class="mui-navigate-right">
							<div class="recordCase">
								<div class="recordTop">
									<div class="recordLeft">
										<div class="recordTitle">
											<span class="imgTxt" v-if="item.consultType === 'ImageText'"></span>
											<span class="imgPhone" v-if="item.consultType === 'Phone'"></span>
											<p class="font-gray">{{item.patientName}} {{item.sex | sexSelect}} {{item.age}} {{item.iiInessType}}</p>
											<p class="font-overflow">{{item.iiInessDescription}}</p>
										</div>
									</div>
									<div class="recordRight">
										<!-- <div class="red-hint">超时</div> -->
										<div v-if="item.consultStateCode!=statusSuccess" class="red-hint hint">{{item.consultState}}</div>
										<!-- <div v-if="item.consultStateCode===statusSuccess && !item.isReturnVisit"  class="red-hint hint">未回访</div>
										<div v-if="item.consultStateCode===statusSuccess && item.isReturnVisit"  class="green-hint hint">已回访</div> -->
										<div v-if="item.consultStateCode===statusSuccess && !item.isEvaluate"  class="red-hint hint">未评价</div>
										<div v-if="item.consultStateCode===statusSuccess && item.isEvaluate"  class="green-hint hint">已评价</div>
										<!-- <div class="record-icon">
											<img v-if="item.consultType === 'ImageText'" src="../../assets/image/chat-icon.png" />
											<img v-if="item.consultType === 'Phone'" src="../../assets/image/phone-icon2.png" />
										</div> -->
									</div>
									
								</div>
								<div class="recordBottom">
									<div class="userTime">
										<p v-if="item.consultType != 'Phone'">剩余{{item.hasInquiryTimes}}次追问</p>
										<p v-if="activeShow && item.createdOnFormatter!=''" class="text-right">已等待{{item.createdOnFormatter}}</p>
										<p v-if="!activeShow && item.createdOnFormatter!=''" class="text-right">距离结束剩余{{item.createdOnFormatter}}</p>
									</div>
								</div>
							</div>
						</router-link>
					</li>
				</ul>  
			</mt-loadmore>
			<div v-if="nomoreShow && listShow" class="noMore">
				<img src="../../assets/image/meiyou.png" alt="">
				<p>暂无待处理咨询~</p>
			</div>
			<!-- <div v-if="addShow" class="addHeight"></div> -->
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
import { createSecret } from 'assets/js/common.js'
import { Tabbar, TabItem, Toast } from 'mint-ui';
let moment = require('moment');
export default {
    data () {
        return {
            iIInessDescription: '',
			selected: 'nav1',
			activeShow: true,
			addShow: false,
			recordList: [],
			recordList1: [],
			hasReply: 1,
			statusSuccess: 'success',
			skipCount: 1,
			skipCount1: 1,
			maxResultCount: 10,
			totalCount: 0,
			totalPage: 2,
			totalPage1: 2,
			loading: true,
			nomoreShow: false,
			homeTop: 0,
			listShow: true,
            allUseLoad: false,
            allUseLoad1: false,
            bottomText: '上拉加载更多...',
        }
	},
	filters: {
        sexSelect (value) {
            const sexStatus = ['', '男', '女']
            return sexStatus[value]
        }
    },
	methods: {
		// goSearch () {
		// 	const iIInessDescription = this.iIInessDescription
		// 	if (!iIInessDescription) {
		// 		Toast('请输入搜索内容')
		// 		return
		// 	}
		// 	this.$router.push({ 
		// 		path: '/doctor-search',
		// 		query: {
		// 			iIInessDescription
		// 		}
		// 	})
		// },
        selectType(flag, index) {
			if (this.activeShow === flag) return
			this.activeShow = flag
			this.listShow = flag
			document.body.scrollTop = 0
            // this.$refs.srcollBox.scrollTop = 0
		},
		// searchKeypress(event) {
        //     if (event.keyCode == 13) {
		// 		this.goSearch()
        //     } 
		// },
		getOrderList(nub) { // 获取咨询列表
			// this.loading = true
			let _this = this
			const maxResultCount  = this.maxResultCount 
			const hasReply = nub
			let skipCount = 0
            let totalPage = 0
            if (nub === 1) {
                skipCount = this.skipCount1
                totalPage = this.totalPage1
            } else {
                skipCount = this.skipCount
                totalPage = this.totalPage
            }
            this.instance.consultationPageD({
				hasReply,
				skipCount,
				maxResultCount
            })
                .then((response) => {
					this.$refs.loadmore.onBottomLoaded()
                    this.$refs.loadmore1.onBottomLoaded()
					if (response.data.result.item.items) {
						let recordList2 = response.data.result.item.items
						if(hasReply === 1) {
							_this.recordList1 = _this.recordList1.concat(recordList2)
							_this.totalCount = response.data.result.item.totalCount
							_this.totalPage1 = response.data.result.item.totalPage
						} else {
							_this.recordList = _this.recordList.concat(recordList2)
							_this.totalPage = response.data.result.item.totalPage
						}
					} else {
						if (hasReply === 1) {
							this.nomoreShow = true
						} else {
							this.nomoreShow = false
						}
					}
                })
                .catch((error) => {
					this.$refs.loadmore.onBottomLoaded()
                    this.$refs.loadmore1.onBottomLoaded()
                }) 
		},
		loadBottomUse() {
            this.skipCount += 1;
            if (this.skipCount >= this.totalPage) {
				this.allUseLoad = true;
				// this.addShow = true
            }
            setTimeout(() => {
                this.getOrderList(2)
            }, 500);
        },
        loadBottomUse1() {
            this.skipCount1 += 1;
            if (this.skipCount1 >= this.totalPage1) {
                this.allUseLoad1 = true;
            }
            setTimeout(() => {
                this.getOrderList(1)
            }, 500);
        },
	},
	mounted () {
		this.selected = 'nav1'
		this.getOrderList(1)
		this.getOrderList(2)
		
	},

    // activated () {
		
    //     this.$refs.srcollBox.scrollTop = this.homeTop || 0
        
    // },
    // beforeRouteLeave (to, from, next) {
    //     let scrollA = this.$refs.srcollBox
    //     this.homeTop = scrollA.scrollTop || 0
    //     console.log(this.homeTop )
    //     next()
    // }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.index-doctor {
	position: relative;
	height: 100%;
	// .listAbout {
	// 	overflow: auto;
	// 	-webkit-overflow-scrolling: touch;
	// 	position: absolute; left: 0; right: 0; top: 42px; bottom: 0px;
	// }
	.listAbout{
		position: absolute;
		left:0;
		top: 0;
		width:100%;
		height:100%;
		box-sizing: border-box;
		overflow:auto;
		padding: 42px 0 55px 0;
		box-sizing:border-box;
		-webkit-overflow-scrolling: touch;
	}
	.addHeight {
		width: 100%;
		height: 55px;
	}
	.noMore {
		position: absolute;
		top: 100px;
		left: 50%;
		margin: 0px 0 0 -100px;
		width: 200px;
		height: 200px;
		p {
			position: absolute;
			left: 0;
			right: 0;
			bottom: 17px;
			text-align: center;
			font-size: 14px;
			color: #999;
		}
		img {
			display: block;
			width: 100%;
			height: 100%;
		}
	}
	.mint-header-title {display: none;}
	.mint-header-button {text-align: center;}
	.activeCheck {color: $color-star;}
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
	.recordTitle span { width: 18px; height: 18px; position: absolute; top:-2px; left: -21px; font-size: $font-m;
	display: inline-block; border-radius: 9px; line-height: 19px; text-align: center;}
	.recordTitle .imgTxt {
			background-repeat: no-repeat;
        	background: url(../../assets/image/write03.png) no-repeat;
        	background-size: auto 18px ;
		}
	.recordTitle .imgPhone {
			background-repeat: no-repeat;
        	background: url(../../assets/image/phone033.png) no-repeat;
        	background-size: auto 18px ;
		}
	.recordBottom {padding-top: 1px;}
	.userTime {display: flex;}
	.userTime p {flex: 1; font-size: $font-m; color: $color-bfont}

	.font-overflow { 
		width: 250px; 
		margin: 5px 0; 
		font-size: $font-l; 
		overflow: hidden; 
		white-space: nowrap;
    	text-overflow: ellipsis;
	}
}

</style>