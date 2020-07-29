<template>
    <div class="audit-detail padding-top">
        <mt-header fixed title="审核详情">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
            <div class="condition">
				<ul class="paList">
					<li class="border-none">
						<p v-if="recordDetail.consultType === 'ImageText'" class="conditionP1 ">图文咨询</p>
						<p v-if="recordDetail.consultType === 'Phone'" class="conditionP1 ">电话咨询
						</p>
					</li>
					<!-- <li>
						姓名：<span>{{recordDetail.patientName}}</span>
					</li> -->
					<li class="shortLi">
						性别：<span>{{sex}}</span>
					</li>
					<li class="shortLi">
						年龄：<span>{{recordDetail.age}}</span>
					</li>
					<li class="shortLi">
						病种：<span>{{recordDetail.iiInessType}}</span>
					</li>
					<!-- <li>
						所在地：<span>{{recordDetail.patientCity}}</span>
					</li> -->
					<li class="shortLi" v-if="recordDetail.hasAllergic">
						药物过敏：<span>{{recordDetail.allergicHistory}}</span>
					</li>
					<li class="shortLi" v-if="!recordDetail.hasAllergic">
						药物过敏：<span>无</span>
					</li>
					<li>
						病情描述：
					</li>
					<li>
						<p class="illCase" v-html="recordDetail.iiInessDescription" ></p>
						<ul class="imgCaseList">
							<li v-if="item.mediatype === 'image'" class="showImg" v-for="(item, index) in consultationfile" :key="index" >
								<!-- <img v-gallery:groupName :src="item.message" /> -->
                                <!-- <img :src="item.message" preview="0" preview-text=""> -->
                                <img :src="`${item.message.replace(/cos.ap-guangzhou/g, 'picgz')}${compressHead}`" @click="readImgInfo(consultationfile, item.message, 'message')" >
							</li>
							
						</ul>
						<!-- <ul class="imgCaseList">
							<li v-if="item.mediatype === 'video'" class="showImg" v-for="(item, index) in consultationfile" :key="index" >
								<img @click="videoClick(true, item.message)" src="../../assets/image/videoPlay.jpg" />
							</li>
							
						</ul> -->
					</li>
				</ul>
			</div>
            <div v-for="(item, index) in replys" :key="index" class="chatListCase">
				<div v-if="item.replyType==='inquiries'" class="zhuiWen">
					<div>追问:
						<span>{{item.createdOn}}</span>
						<p></p>
						<p v-html="item.message" class="zhuiContent"></p>
						<ul class="replaysBox replaysBox1">
							<li  v-if="item1.mediaType === 'image'" v-for="(item1, index1) in item.consultationFile" :key="index1">
								<!-- <img v-gallery:groupName :src="item1.fileUrl" /> -->
                                <!-- <img :src="item1.fileUrl" preview="1" preview-text=""> -->
                                <img :src="`${item1.fileUrl.replace(/cos.ap-guangzhou/g, 'picgz')}${compressHead}`" @click="readImgInfo(item.consultationFile, item1.fileUrl, 'fileUrl')" >
							</li>
							<!-- <li v-if="item1.mediaType === 'video'" v-for="(item1, index1) in item.consultationFile" :key="index1">
								<video width="100" controls :src="item1.fileUrl" height="100" autoplay="none"></video>
							</li> -->
						</ul>
						
					</div>
				</div>
				<div v-if="item.replyType==='answer'"  class="weChat">
					<div class="weTime">
						<div>{{item.createdOn}}</div>
					</div>
					
					<div v-if="item.answerType === 'Phone'" class="weTitle">
						<!-- <router-link :to="{path: '/doctor-detail-patient', query: {id: recordDetail.doctorID}}">
							<img class="avatar" :src="recordDetail.userImage" />
						</router-link> -->
						<p>
							医生在 {{item.createdOn}} 进行了电话回复
						</p>
					</div>
					<div v-if="item.answerType === 'Message'" class="weTitle">
						<!-- <router-link :to="{path: '/doctor-detail-patient', query: {id: recordDetail.doctorID}}">
							<img class="avatar" :src="recordDetail.userImage" />
						</router-link> -->
						<p v-html="item.message">
						</p>
						<p>
							<ul class="replaysBox">
								<li class=""  v-if="item1.mediaType === 'image'" v-for="(item1, index1) in item.consultationFile" :key="index1">
									<!-- <img v-gallery:groupName :src="item1.fileUrl" /> -->
                                    <!-- <img :src="item1.fileUrl" preview="2" preview-text=""> -->
                                    <img :src="`${item1.fileUrl.replace(/cos.ap-guangzhou/g, 'picgz')}${compressHead}`" @click="readImgInfo(item.consultationFile, item1.fileUrl, 'fileUrl')" >
								</li>
								<!-- <li class="" v-if="item1.mediaType === 'video'" v-for="(item1, index1) in item.consultationFile" :key="index1">
									<video width="100" controls :src="item1.fileUrl" height="100" autoplay="none"></video>
								</li> -->
								<li class="voiceLi" v-if="item1.mediaType === 'voice'" v-for="(item1, index1) in item.consultationFile" :key="index1">
									<span class="lengthSpan" v-for="(value, key) in item1.fileTotalTime" :key="key"></span><span @click="playAudio(index, item1.fileUrl, item1.fileTotalTime, item1.play)" class="voiceSpan" :class="{voicePlay: item1.play}">
									</span>
									<span class="timeVoice">{{item1.fileTotalTime}}''</span>
								</li>
							</ul>
						</p>
					</div>
				</div>
			</div>
            <div class="doctorDot">
                <div v-if="recordDetail.replyState === 'untreated'" class="flex">
                    <div class="star-control">
                        您的打分：
                        <div class="give-star">
							<div @click="playStar(index)" v-for="(value,index) in 5" :key="index" :class="{starOk: value <= giveStar, starNo: value > giveStar}"></div>
						</div>
                    </div>
                    <div class="delete-btn">
                        <mt-button  @click="closeOrder" type="default">退回订单</mt-button>
                    </div>
                </div>
                <div v-if="recordDetail.replyState === 'treated'">
                    <div class="replys1">
                        <p class="hint green-hint">我的评价：<star :star="recordDetail.qualityLevel"></star></p>
                        <p>{{recordDetail.repayIllnessDescription}}</p>
                    </div>
                </div>
                <div v-if="recordDetail.replyState === 'untreated'" class="evaluateCase">
                    <p>评价描述：</p>
                    <div class="mui-input-row evaluate-row">
                        <mt-field label=""  @input = "descInput" placeholder="填写评价信息" :attr="{ maxlength: maxReplyLength }" type="textarea" rows="5" v-model="repayIllnessDescription"></mt-field>
                        <span class="font-number">可输入{{remark}}字</span>
                    </div>
                    <div class="okCase">
                        <mt-button @click="submitAudit" type="primary" class="mint-button--large">提交</mt-button>
                    </div>
                </div>
            </div>
        </div>
        <div v-show="false">
			<audio ref="audioCase" id="audioCase" src=""></audio>
		</div>
    </div>
</template>

<script>
import Star from 'components/star/star'
import { Toast } from 'mint-ui';
let moment = require('moment');
export default {
    components: {
      Star
    },
    data () {
        return {
            id: 0,
            maxReplyLength: 500,
            giveStar: 0,
            remark: 500,
            repayIllnessDescription: '',
            recordDetail: {},
            consultationfile: [],
            replys: [],
            paramsType: {},
            show: false,
			returnShow: true,
			videoSrc: '',
			videoShow: false,
			sex: '',
            refundReason: '',
            compressHead: '?imageView2/q/20'
        }
    },
    filters: {
        sexSelect (value) {
            const sexStatus = ['', '男', '女']
            return sexStatus[value]
        }
    },
	methods: {
        readImgInfo(listImg, value, str) {
            let imgList = []
            for (var i = 0; i < listImg.length; i ++) {
				let obj = ''
				if (str === 'src') {
					obj = listImg[i].src
				} 
				if (str === 'message') {
					obj = listImg[i].message
				}
				if (str === 'fileUrl') {
					obj = listImg[i].fileUrl
				}
				imgList.push(obj)
            }
            console.log(imgList)
            WeixinJSBridge.invoke("imagePreview", {
                current: value,
                urls: imgList
            });
		},
        descInput() {
            let txtVal = this.repayIllnessDescription.length;
            this.remark = this.maxReplyLength - txtVal;
        },
        playStar (index) { // 打分
			this.giveStar = index + 1
        },
        videoClick(flag, video) {
            this.videoShow = flag
            if (flag) {
				this.videoSrc = video
				let videoS = document.getElementById("videoSelf")
                videoS.play()
            } 
            
		},
		playAudio (index, src, time, playFlag){ // 音频控制在
			let _this= this
			let timerS
			clearTimeout(timerS)
			timerS = setTimeout(function() {
				_this.replys[index].consultationFile[0].play = false
			}, time*1000)
            let audioCase1 = document.getElementById('audioCase')
			this.$refs.audioCase.src = src
            setTimeout(function() {
                if (playFlag) {
                    _this.replys[index].consultationFile[0].play = false
                    audioCase1.pause()
                } else {
					audioCase1.play()
                    for(let i = 0; i < _this.replys.length; i++) {
                        if (index === i) {
                            _this.replys[index].consultationFile[0].play = true
                        } else {
							if(_this.replys[i].consultationFile.length != 0) {
								if (_this.replys[i].consultationFile[0].mediaType === 'voice') {
									_this.replys[i].consultationFile[0].play = false
								}
							}
                        }
                    }
                }
            }, 100)
        },
        closeOrder () { // 退回订单
            const id = this.id
            const replyState = this.paramsType[2].code
            this.instance.qualityControlManageD({
                id,
                replyState
            })
                .then((response) => {
                    Toast('退回成功')
                    this.$router.push({ 
                        path: '/audit-process'
                    })
                })
                .catch((error) => {
                }) 
            
        },
        getOrderDetail() { // 获取订单详情
			const id = this.id
            this.instance.qualityControlManageDetailD({
				id
            })
                .then((response) => {
					let recordDetail = response.data.result.item
					this.recordDetail = recordDetail
					this.replys = recordDetail.replys
                    this.consultationfile = recordDetail.consultationfile
                    this.recordDetail.iiInessDescription = this.recordDetail.iiInessDescription.replace(/\n/g,"<br/>")
					const sex = recordDetail.sex
					const sexStatus = ['', '男', '女']
					this.sex = sexStatus[sex]
					for (let i = 0; i < this.replys.length; i++) {
                        this.replys[i].createdOn = moment(this.replys[i].createdOn).format('YYYY-MM-DD HH:mm:ss')
                        this.replys[i].message = this.replys[i].message.replace(/\n/g,"<br/>")
						if (!this.replys[i].consultationFile) this.replys[i].consultationFile = []
						if(this.replys[i].consultationFile.length != 0) {
							for (let j = 0; j < this.replys[i].consultationFile.length; j++) {
								if (this.replys[i].consultationFile[0].mediaType === 'voice') {
									this.$set(this.replys[i].consultationFile[0], 'play', false)
									let fileTotalTime1 = this.replys[i].consultationFile[0].fileTotalTime	
									this.replys[i].consultationFile[0].fileTotalTime = Math.ceil(fileTotalTime1)
								}
							}
						}
                    }
                    console.log(this.replys)
                    this.$previewRefresh()
					if (recordDetail.refundReason) {
						this.refundReason = JSON.parse(recordDetail.refundReason).LabelName
					}
                })
                .catch((error) => {
                }) 
        },
        replyParameter () { // 获取审核参数
            this.instance.yaeherPatientParameterListD({
                Type: 'ConfigPar',
                SystemCode: 'QualityControlManageState'
            })
                .then((response) => {
					this.paramsType = response.data.result.item
                })
                .catch((error) => {
                }) 
        },
        submitAudit () { // 提交评价
            const id = this.id
            const qualityLevel = this.giveStar
            const replyState = this.paramsType[1].code
            const repayIllnessDescription = this.repayIllnessDescription

            if(qualityLevel <= 0) {
                Toast('请进行打分')
                return
            }

            if(!repayIllnessDescription) {
                Toast('评价信息不能为空')
                return
            }

            this.instance.qualityControlManageD({
                id,
                qualityLevel,
                replyState,
                repayIllnessDescription
            })
                .then((response) => {
                    Toast('提交成功')
                    this.$router.push({ 
                        path: '/audit-process'
                    })
                })
                .catch((error) => {
                }) 
        }
	},
	mounted () {
        this.getOrderDetail()
        this.replyParameter()
	},
	created () {
        this.id = parseInt(this.$route.query.id)
        this.maxReplyLength = parseInt(window.sessionStorage.getItem('maxReplyLength'))
        this.remark = this.maxReplyLength

	}
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.audit-detail {
    .doctorDot {
        background: #fff;
        padding-top: 20px;
        .flex {
            padding: 0 10px;
            .star-control {
                padding-top: 13px;
            }
            .delete-btn {
                text-align: right;
            }
        }
        
    }
    .evaluateCase {
        background: #fff;
        padding: 10px;
        .evaluate-row {
            position: relative;
            padding-bottom: 10px;
            .font-number {
                position: absolute;
                bottom: 0;
                right: 10px;
                font-size: $font-l;
            }
        }
        .okCase {
            padding: 20px 0;
        }
        .mint-field .mint-cell-title {
            width: 50px;
        }
    }
    .replys1 {
        padding: 20px 10px;
        .hint {
            padding-bottom: 3px;
        }
    }
    p {
        word-wrap: break-word;
    }
    .condition {margin: 10px 0 0; background: $color-wfont; font-size: $font-l; color: #999;}
	.conditionP {padding-bottom: 10px;}
	.condition span {color: $color-afont;}
	.conditionP1 span {font-size: $font-m; background: $default-color; color: #fff; float: right; display: inline-block; width: 60px; height: 20px;text-align: center;line-height: 20px;border-radius: 3px;}
	.condition .paList li {border-top: 1px dashed #eee; padding: 10px 0; border-bottom: none;}
	.condition .paList .border-none {border: none;}
	img {border-radius: 3px;}
    .zhuiWen { padding: 10px 15px; background: #fff;}
	.zhuiWen div {padding: 10px 0; font-size: $font-l; color: $color-bfont;}
	.zhuiWen div span{float: right;}
	.zhuiWen .zhuiContent {color: $color-afont;}
	.zhuiWen img { width: 100px;}
	.replaysBox {
		// overflow: hidden;
		padding: 0 0 0 5px;
		background: $default-color;
		li {
			float: left;
			width: 80px;
			height: 80px;
			padding: 5px 5px 5px 0;
			img{
				width: 100%;
				height: 100%;
				border-radius: 5px;
			}
			video {
				width: 100%;
				height: 100%;
			}
		}
		.voiceLi {
			width: auto;
			height: 20px;
			padding: 0;
			position: relative;
			.timeVoice {
				position: absolute;
				top: 0;
				left: -40px;
				color: $color-bfont;
				text-align: right;
			}
		}
	}
	.voiceSpan{
        display: inline-block;
        width: 20px;
        height: 20px;
        background-repeat: no-repeat;
        background: url(../../assets/image/audioBg.png) no-repeat;
        background-size: auto 20px ;
		background-position: -55px 0px;
		
		
        // background-position: -35px 0px;
        // background-position: -17px 0px;
        // background-position: 0px 0px;
        // background-color: #111;
    }
    .voicePlay{
        /*播放中（不需要过渡动画）*/
        /*background-position: 0px -0px;*/
        -webkit-animation: voiceAnimitation 0.8s infinite step-start;
        -moz-animation: voiceAnimitation 0.8s infinite step-start;
        -o-animation: voiceAnimitation 0.8s infinite step-start;
        animation: voiceAnimitation 0.8s infinite step-start;
	}
	
	@keyframes voiceAnimitation {
        0%,
        100% {
        background-position: -35px 0px;
        }
        33.333333% {
        background-position: -17px 0px;
        }
        66.666666% {
        background-position: 0px 0px;
        }
    }

    @-webkit-keyframes voiceAnimitation {
        0%,
        100% {
        background-position: -35px 0px;
        }
        33.333333% {
        background-position: -17px 0px;
        }
        66.666666% {
        background-position: 0px 0px;
        }
	}
	.replaysBox1 {
		overflow: hidden;
		background: #fff;
		li {
            border: none;
            width: 95px;
			height: 95px;
			padding: 5px 9px 5px 0;
		}
    }

    .paList {padding: 0 10px; overflow: hidden;}
    .paList li {border-top: 1px dashed #eee; width: 100%; float: left; padding: 10px 0; border-bottom: none;}
    .paList .shortLi {width: 50%;}
    .paList .illCase {
		line-height: 20px;
		padding: 0 10px; 
		display: block;
	}
	.paList .imgCaseList {padding: 10px 0; overflow: hidden;}
	.paList .imgCaseList .showImg {float: left; width: 100px; height: 100px; margin: 5px 9px 0px 0; padding: 0; overflow: hidden;}
	.paList .imgCaseList .showImg:nth-child(3n) {padding-right: 0px;}
	.paList .imgCaseList .showImg {
		position: relative;
		img{
			width: 100%;
            position: absolute;
            top: 50%;
            left: 50%;
            display: block;
            min-width: 100%;
            min-height: 100%;
            transform:translate(-50%,-50%);
            border-radius: 5px;
        }
        video{
			width: 100%;
            position: absolute;
            top: 50%;
            left: 50%;
            display: block;
            min-width: 100%;
            min-height: 100%;
            transform:translate(-50%,-50%);
            border-radius: 5px;
        }
	}	
    .weChat {padding: 10px; background: #fff; overflow: hidden;}
	.weTime {width: 100%; float: right; display: flex; justify-content: center; align-items: center; padding-bottom: 10px;}
	.weTime div {padding: 3px 5px; background: #aaa; color: $color-wfont; font-size: $font-l; border-radius: 3px;}
	.weChat p {font-size: $font-l; color: #333;}
	.weTitle {background: $default-color;display: inline-block; float: right; padding: 10px; margin-right: 50px; position: relative; border-radius: 5px;}
	.weTitle img {width: 80px;}
	.weTitle .avatar {position: absolute; right: -50px; top: 0; width: 40px; height: 40px; border-radius: 5px; padding: 0;}
	.weTitle p {width: 100%;word-break: break-all;}
}

</style>