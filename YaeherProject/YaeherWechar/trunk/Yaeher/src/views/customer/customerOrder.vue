<template>
    <div class="customer-order">
		<mt-header fixed title="订单查看">
            <a @click="$router.push({path: '/customer-user'})" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div ref="srcollBox" class="content listAbout">
            <div class="searchCase">
                <div class="searchBox">
                    <input @keypress="searchKeypress" ref="inputText" type="search" v-model="keyWord" placeholder="请输入关键字查询" />
                    <div @click="getrecordList(true)">搜索</div>
                </div>
            </div>
            <ul v-infinite-scroll="getrecordList"
  				infinite-scroll-disabled="loading"
  				infinite-scroll-distance="10"  class="mui-table-view recordList record-list-user">
				<li v-for="(item, index) in recordList" :key="index" class="mui-table-view-cell">
			        <router-link :to="{path: '/customer-detail', query: {id: item.id}}" class="mui-navigate-right">
			        	<div class="recordCase">
			        		<div class="recordTop">
			        			<div class="recordLeft">
			        				<div class="recordAvatar">
				        				<img :src="item.userImage" />
										<span class="imgTxt" v-if="item.consultType === 'ImageText'"></span>
										<span class="imgPhone" v-if="item.consultType === 'Phone'"></span>
				        			</div>
				        			<div class="recordTitle">
				        				<p class="doctor-name">{{item.doctorName}}</p>
				        				<p>{{item.iiInessType}}</p>
				        			</div>
				        			
			        			</div>
			        			<div class="recordRight">
									<div v-if="item.consultStateCode!=statusSuccess" class="red-hint hint">{{item.consultState}}</div>
									<!-- <div v-if="item.consultStateCode===statusSuccess"  class="green-hint">{{item.consultState}}</div> -->
									<!-- <div v-if="item.consultStateCode===statusSuccess && !item.isReturnVisit"  class="red-hint hint">未回访</div>
									<div v-if="item.consultStateCode===statusSuccess && item.isReturnVisit"  class="green-hint hint">已回访</div> -->
									<div v-if="item.consultStateCode===statusSuccess && !item.isEvaluate"  class="red-hint hint">未评价</div>
									<div v-if="item.consultStateCode===statusSuccess && item.isEvaluate"  class="green-hint hint">已评价</div>
									<div class="record-icon">
										<!-- <img v-if="item.consultType === 'ImageText'" src="../../assets/image/chat-icon.png" />
										<img v-if="item.consultType === 'Phone'" src="../../assets/image/phone-icon2.png" /> -->
									</div>
			        			</div>
			        			
			        		</div>
			        		<div class="recordBottom">
			        			<div class="userTime">
			        				<p>咨询人：{{item.patientName}}</p>
			        				<p class="text-right">{{item.createdOn}}</p>
			        			</div>
			        		</div>
			        	</div>
			        </router-link>
			  </li>
			  <!-- <li v-if="moreShow" class="noneMore">没有更多了~~</li> -->
			</ul>   
        </div>
    </div>
</template>

<script>
import Bscroll from 'better-scroll'
import { Toast } from 'mint-ui';
var moment = require('moment')
export default {
    data () {
        return {
			id: 0,
			recordList: [],
			statusSuccess: 'success',
			createdOn: '',
			maxResultCount: 10,
			skipCount: 1,
			totalPage: 2,
			homeTop: 0,
            loading: true,
            keyWord: ''
        }
	},
	methods: {
        searchKeypress() {
            if (event.keyCode == 13) {
                this.getrecordList(true)
            } 
        },
		getrecordList (flag) {
            if (flag) {
                this.skipCount = 1
                this.totalPage = 2
            }
            this.loading = true
			const keyWord = this.keyWord
			
			const skipCount = this.skipCount
			const id = this.id
			const maxResultCount  = this.maxResultCount 
			if(skipCount > this.totalPage) {
				if(skipCount > 2) {
                    Toast('没有更多了！')
                }
				return
			}
            this.instance.consultationPageD({
                keyWord,
				skipCount,
				maxResultCount
            })
                .then((response) => {
					setTimeout(() => {
                        this.loading = false
                        if (flag) this.recordList = []
						let recordList1 = response.data.result.item.items
						if (!recordList1 || recordList1.length===0) {
							this.loading = true
                        } else {
                            this.recordList = this.recordList.concat(recordList1)
                            this.skipCount ++
                            this.totalPage = response.data.result.item.totalPage
                            console.log(this.recordList)
                            for (let i = 0; i < this.recordList.length; i++) {
                                this.recordList[i].createdOn = moment(this.recordList[i].createdOn).format('YYYY-MM-DD HH:mm:ss')
						    }
                        }
						
					}, 200);	
					
                })
                .catch((error) => {
					this.loading = true
                }) 
		}
	},
	mounted () {
		this.getrecordList()
	},
	created () {
		this.id = parseInt(window.sessionStorage.getItem('userId'))
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

.customer-order {
	-webkit-user-select: text;
    -moz-user-select: text;
    -ms-user-select: text;
	user-select: text;
	.listAbout {
        position: absolute; left: 0; right: 0; bottom: 0px;top: 42px;
    }
	.recordCase {padding: 0px 10px; font-size: 12px;}
	.record-list-user {background: #f0f0f0; padding: 10px 10px;}
	.recordList .mui-table-view-cell:after {height: 5px;}
	// .recordList li {border-bottom: 5px solid #f0f0f0;}
	.record-list-user li {margin-bottom: 10px; background: $color-wfont; border: none; border-radius: 3px;}
	.recordTop {display: flex; border-bottom: 1px solid #eee; padding-bottom: 5px;}
	.recordRight {width: 50px; color: $color-green; text-align: right;}
	.recordRight .hint {padding-bottom: 3px;}
	.recordRight > div {padding-bottom: 5px;}
	.recordLeft {flex: 1; display: flex;}
	.recordAvatar {width: 70px; position: relative;}
	.recordAvatar span { 
		width: 20px; 
		height: 20px; 
		// color: $color-wfont; 
		// background-color: $color-red; 
		position: absolute; 
		top: -5px; 
		right: 12px; 
		font-size: $font-m;
		display: inline-block; 
		border-radius: 9px; 
		line-height: 19px; 
		text-align: center;
		}
	.recordAvatar .imgTxt {
			background-repeat: no-repeat;
        	background: url(../../assets/image/write03.png) no-repeat;
        	background-size: auto 18px ;
		}
	.recordAvatar .imgPhone {
			background-repeat: no-repeat;
        	background: url(../../assets/image/phone033.png) no-repeat;
        	background-size: auto 18px ;
		}
	.recordAvatar img {width: 50px; height: 50px; border-radius: 5px;}
	.recordTitle {flex: 1; text-align: left;}
	.recordBottom {padding-top: 10px;}
	.userTime {display: flex;}
	.userTime p {flex: 1;}
	.doctor-name {font-size: $font-xl}
	.noneMore {text-align: center;}
}

</style>