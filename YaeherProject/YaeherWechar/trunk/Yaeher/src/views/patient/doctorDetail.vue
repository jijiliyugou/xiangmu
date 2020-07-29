<template>
    <div class="doctor-detail-patient">
		<!-- <mt-header fixed :title="doctorDetail.doctorName">
            <a v-if="rShow != 'no'" @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header> -->
        <div class="content">
            <div class="doctorInfo1">
				<div @click="collectDoctor" class="collection" :class="{collect_no: !doctorDetail.isCollect}"></div>
				<img v-if="doctorDetail.userImageFile === null" src="../../assets/image/doctorDeafult.jpg"/>
				<img v-if="doctorDetail.userImageFile != null" :src="doctorDetail.userImageFile"/>
				<div class="grayBg"></div>
				<div class="infoCase">
					
					<p class="doctorName">{{doctorDetail.doctorName}}</p>
					<p style="padding-top: 5px;"><star :star=doctorDetail.averageEvaluate></star><span v-if="doctorDetail.averageEvaluate!=0"> {{doctorDetail.averageEvaluate}}</span></p>
					<!-- <p>已回答：{{doctorDetail.receiptNumBer}}个咨询</p> -->
					<!-- <p>平均{{doctorDetail.averageTime}}小时回复</p> -->
					<p>{{doctorDetail.hospitalName}} {{doctorDetail.title}} 从业{{doctorDetail.workYear}}年</p>
				</div>
			</div>
            <div class="diseaseInfo">
				<span class="labelSpan" v-for="(item, index) in doctorDetail.lableManages" :key="index">
					{{item.lableName}}
				</span>
			</div>
            <div class="blackTop">
				<ul class="consult-list">
					<li v-for="(item, index) in serviceMoneyLists" :key="index" class="consultList" :class="{textAult: item.serviceType==='ImageText', phoneAult: item.serviceType==='Phone'}">
						<div v-if="item.serviceType==='ImageText'" class="consultLeft">图文咨询</div>
						<div v-if="item.serviceType==='Phone'" class="consultLeft">电话咨询</div>
						<div class="consultRight">
							￥<b>{{item.serviceExpense}}</b><span v-if="item.serviceType==='Phone'">/{{item.serviceDuration}}分钟</span>
							<router-link v-if="item.serviceState && item.receiptState" :to="{path: '/agreement', query: {id: item.id, price: item.serviceExpense, serviceType: item.serviceType}}">
                                <mt-button class="priceBtn" type="primary" size="small">咨询</mt-button>
                            </router-link>
							<a v-if="!item.receiptState && item.serviceState" href="javascript:;">
                                <mt-button class="disAble" type="default" size="small">已满额</mt-button>
                            </a>
							<a v-if="!item.serviceState" href="javascript:;">
                                <mt-button class="disAble" type="default" size="small">暂停服务</mt-button>
                            </a>
						</div>
						<span v-if="item.serviceType==='Phone'" class="phoneHint">注：海外及新疆地区电话号码暂不支持</span>
				  	</li>
				  	<!-- <li v-if="doctorScheduling.length!=0" class="consultList">
					 	<router-link :to="{path: '/doctor-scheduling', query: {id}}" style="width: 100%;" href="../doctor/arrange.html" class="mui-navigate-right">
					 		门诊排班
					 	</router-link>
				  	</li> -->
				</ul>  
			</div>
            <div class="blackTop blackCase">
				<div class="icon-case"></div>
				<div class="blackTitle">简介</div>
				<!-- <pre>{{doctorDetail.doctorIntroduce}}</pre> -->
				<div ref="introduceCase" class="doctorIntroduceCase" :class="{maxIntroduceCase: iShow}"  v-html="doctorDetail.doctorIntroduce"></div>
				<div v-if="heightCase === 200" @click="introduceShow" class="openIntroduce">
					<div class="arrowBottom" :class="{arrowTop: iShow}" ></div>
				</div>
			</div>	
			<div class="blackTop blackCase">
				<div class="icon-case icon-case1"></div>
				<div class="blackTitle">执业经历</div>
				<p v-for="(item, index) in doctorDetail.doctorEmployment" :key="index">{{item.hospitalName}}/{{item.department}}</p>
			</div>
            <div v-if="doctorPapers.length!=0" class="listTitle relative">
				发表的文章
				<router-link  :to="{path: '/article-list', query: {id}}">查看更多>></router-link>
			</div>
			<ul v-if="doctorPapers.length!=0" class="">
				<li v-if="index<3" v-for="(item, index) in doctorPapers" :key="index" class="">
			        <router-link :to="{path: '/article-detail', query: {id: item.id}}" class="articleList">
						<img v-if="item.imageFie !== null" :src="item.imageFie"  alt="" />
                    	<img v-if="item.imageFie === null" src="../../assets/image/article3.jpg" alt="" />
			        	<div class="articleCase">
			        		<p class="blod">{{item.paperTiltle}}</p>
			        		<p class="line-text">{{item.paperContent}}</p>
			        	</div>
			        </router-link>
			    </li>
			</ul>
            <!-- <div class="listTitle relative">
				发表的课程
				<router-link  :to="{path: '/article-list', query: {id}}">查看更多>></router-link>
			</div>
            <ul class="">
				<li class="">
			        <a class="articleList" href="doctorDetail.html">
			        	<img src="../../assets/image/article3.jpg" align="articleList" />
			        	<div class="articleCase">
			        		<p class="blod">教孩子吃饭</p>
			        		<p>课程时间：30分钟</p>
			        		<p>这是一个教孩子吃饭的视频</p>
			        	</div>
			        </a>
			  	</li>
			  	<li class="">
			        <a class="articleList" href="doctorDetail.html">
			        	<img src="../../assets/image/article1.jpg" align="articleList" />
			        	<div class="articleCase">
			        		<p class="blod">教孩子吃饭</p>
			        		<p>课程时间：30分钟</p>
			        		<p>这是一个教孩子吃饭的视频</p>
			        	</div>
			        </a>
			  	</li>
			</ul> -->
        </div>
    </div>
</template>

<script>
import { Toast } from 'mint-ui';
import { createSecret } from 'assets/js/common.js'
import Star from 'components/star/star'
export default {
	components: {
      Star
    },
    data () {
        return {
			id: 0,
			userId: 0,
			show: true,
			iShow: false,
			heightCase: 0,
			rShow: '',
			doctorIntroduce: '',
			doctorDetail: {},
			serviceMoneyLists: [],
			doctorScheduling: [],
			doctorPapers: []
        }
	},
	methods: {
		introduceShow () {
			this.iShow = !this.iShow
		},
		collectDoctor() {
			this.collectDoctor()
		},
		getDoctorDetail() { // 获取医生详情
			const id = this.id
            this.instance.clinicDoctor({
				id,
				maxResultCount: 3
            })
                .then((response) => {
					this.doctorDetail = response.data.result.item
					document.title = this.doctorDetail.doctorName
					const statusArry = ['', '图文咨询', '电话咨询']
					window.sessionStorage.setItem('doctorId', this.doctorDetail.id)
					
					// window.sessionStorage.setItem('doctorIdU', this.doctorDetail.userID)
					this.serviceMoneyLists = this.doctorDetail.serviceMoneyLists
					this.doctorScheduling = this.doctorDetail.doctorScheduling
					this.doctorDetail.doctorIntroduce = this.doctorDetail.doctorIntroduce.replace(/\n/g,"<br/>")
					this.$nextTick(() => {
						this.heightCase = this.$refs.introduceCase.offsetHeight
					})
					this.doctorPapers = this.doctorDetail.doctorPapers
					for (let i = 0; i < this.doctorPapers.length; i++) {
						this.doctorPapers[i].paperContent = this.doctorPapers[i].paperContent.replace(/<[^>]+>|&/g,"").replace(/nbsp;/g,"")
					}
                })
                .catch((error) => {
                }) 
		},
		collectDoctor() { // 收藏或者取消收藏医生
			const doctorID = this.id
			const userId = this.userId
			const isCollect = this.doctorDetail.isCollect
            this.instance.patientDoctor({
				doctorID,
				userId
            })
                .then((response) => {
					this.getDoctorDetail()
					isCollect ? Toast('取消收藏成功') : Toast('收藏成功')
                })
                .catch((error) => {
                    console.log('收藏医生失败')
                }) 
		},
		
	},
    mounted () {
		this.getDoctorDetail()
		// this.$nextTick(() => {
		// 	console.log(this.$refs.introduceCase)
		// 	let height = this.$refs.introduceCase.offsetHeight
		// 	console.log(height)
		// })
    },
    created () {
		let id = this.$route.query.id
		if (id) {
			this.id = parseInt(id)
			this.rShow = this.$route.query.rShow
		}
		this.userId = parseInt(window.sessionStorage.getItem('userId'))
		window.sessionStorage.setItem('doctorId', this.id)
		
		
	},
	activated () {
		
	}
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";
.doctor-detail-patient {
	-webkit-user-select: text;
	-moz-user-select: text;
	-ms-user-select: text;
	user-select: text;
	.consult-list {
		padding: 0 10px;
	}
	// pre{
	// 	white-space: pre-wrap;
	// 	word-wrap: break-word;
	// 	font-size: $font-l;
	// 	color: $color-bfont;
	// 	margin: 0;
	// 	line-height: 20px;
	// }
	.doctorIntroduceCase {
		font-size: $font-l;
		color: $color-bfont;
		line-height: 22px;
		max-height: 200px;
		overflow: hidden;
	}
	.maxIntroduceCase {
		max-height: 1500px;
	}
	
	.openIntroduce {
		padding: 10px 0;
		display: flex;
		-webkit-tap-highlight-color:rgba(255,255,255,0);
		.arrowBottom{
			display: inline-block;
			width: 10px;
			height: 10px;
			margin: 0 auto;
			border-top: 1px solid #777;
			border-right: 1px solid #777;
			transform: rotate(135deg);

		}

		.arrowTop{
			transform: rotate(-45deg)
		}  
	}
	.doctorInfo1 {
		position: relative; 
		min-height: 200px;
		background: #aaa;
		overflow: hidden;
		.grayBg {
			position: absolute;
			top: 0;
			left: 0;
			right: 0;
			bottom: 0;
			z-index: 2;
			background: rgba(0,0,0,0.1)
		}
		img {
			width: 100%;
			display: block;
		}
		.collection {
			position: absolute; 
			right: 10px; 
			top: 10px; 
			width: 30px; 
			height: 30px; 
			background: url(../../assets/image/collect-ok.png) no-repeat; 
			background-size: 100%;
			z-index: 10;
		}
		.collect_no {
			background: url(../../assets/image/collect-no.png) no-repeat;
			background-size: 100%;
		}
		.infoCase {
			position: absolute; 
			left: 15px; 
			bottom: 10px;
			z-index: 3;
			p {
				color: $color-wfont; 
				font-size: $font-m; 
				margin-bottom: 0; 
				line-height: 16px;
			}
			.doctorName {
				font-size: $font-xxl;
			}
		}
	}


	.diseaseInfo {
		padding: 10px; 
		background: $color-wfont;
		// font-size: $font-l; 
		.labelSpan {
			display: inline-block; 
			margin-top: 7px;
			margin-right: 12px; 
			padding: 5px 4px; 
			font-size: $font-m; 
			color: $color-bfont; 
			line-height: 14px;
			height: 12px; 
			border-radius: 10px; 
			border: 1px solid #ccc;
		}
	}

	.blackTop {
		margin-top: 10px; 
		background: $color-wfont;
		.consultList {
			display: flex; 
			padding: 20px 0 20px 30px;
			position: relative;
			.consultLeft {
				text-align: left; 
				width: 70px;
				line-height: 34px;
				
			}
			.consultRight {
				flex: 1; 
				text-align: right; 
				vertical-align: middle; 
				font-size: $font-l; 
				color: $color-star;
				.priceBtn {
					padding: 0 26px;
					background-color: #ff8518;
				}
				.disAble {
					background-color: #ccc;
					label {
						color: #fff;
					}
				}
				.mint-button {
					margin-left: 5px;
				}
			}
			.phoneHint {
				position: absolute;
				bottom: 0;
				font-size: 11px;
				line-height: 12px;
				padding-bottom: 3px;
				color: #aaa;
			}
		}
		.textAult {
			background: url(../../assets/image/testConsult.png) 0px 26px no-repeat; 
			background-size: 20px;
		}
		.phoneAult {
			background: url(../../assets/image/phoneConsult.png) 0px 26px no-repeat; 
			background-size: 20px;
		}
	}

	.consultList > div { 
		line-height: 30px;
		}


	.blackCase {
		padding: 20px 10px 20px 40px; 
		background: $color-wfont; 
		position: relative;
		.blackTitle {
			font-size: $font-xl; 
			color: $color-afont; 
			padding-bottom: 5px;
		}
		p {
			color: $color-bfont; 
			margin-bottom: 0; 
			font-size: $font-l;
		}
		.icon-case {
			position: absolute; 
			top: 18px; 
			left: 10px; 
			width: 20px; 
			height: 20px; 
			background: url(../../assets/image/experience-icon.png) 0px 0px no-repeat; 
			background-size: 100%;
		}
		.icon-case1 {
			background: url(../../assets/image/experience2-icon.png) 0px 0px no-repeat; 
			background-size: 100%;
		}
	}


	.listTitle {
		font-size: $font-l; 
		padding: 10px 20px;
		a {
			position: absolute; 
			right: 10px; 
			top: 10px; 
			font-size: $font-m; 
			color: $default-color;
		}
	}
	.relative {position: relative;}

	.articleCase p {font-size: $font-l; color: $color-bfont;}
	.articleCase .blod {font-size: $font-xl; color: $color-afont;}
	.articleCase .line-text {width: 250px; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;}
}

</style>