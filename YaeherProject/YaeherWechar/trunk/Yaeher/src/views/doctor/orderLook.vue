<template>
    <div class="orderLook padding-top">
		<mt-header fixed title="咨询详情">
            <a v-if="returnShow" @click="$router.go(-1)" slot="left">
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
					<li class="shortLi">
						姓名：<span>{{recordDetail.patientName}}</span>
					</li>
					<li class="shortLi">
						性别：<span>{{sex}}</span>
					</li>
					<li class="shortLi">
						年龄：<span>{{recordDetail.age}}</span>
					</li>
					<li class="shortLi">
						病种：<span>{{recordDetail.iiInessType}}</span>
					</li>
					<li class="shortLi">
						所在地：<span>{{recordDetail.patientCity}}</span>
					</li>
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
								<img :src="item1.fileUrl" preview="1" preview-text="">
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
						<router-link :to="{path: '/look-detail', query: {id: recordDetail.doctorID}}">
							<!-- <img class="avatar" :src="recordDetail.userImage" /> -->
							<img :src="`${item1.fileUrl.replace(/cos.ap-guangzhou/g, 'picgz')}${compressHead}`" @click="readImgInfo(item.consultationFile, item1.fileUrl, 'fileUrl')" >
						</router-link>
						<p>
							您在 {{item.createdOn}} 进行了电话回复
						</p>
					</div>
					<div v-if="item.answerType === 'Message'" class="weTitle">
						<router-link :to="{path: '/look-detail', query: {id: recordDetail.doctorID}}">
							<img class="avatar" :src="recordDetail.userImage" />
						</router-link>
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
								<li class="voiceLi" @click="playAudio(index, item1.fileUrl, item1.fileTotalTime, item1.play)" v-if="item1.mediaType === 'voice'" v-for="(item1, index1) in item.consultationFile" :key="index1">
									<span class="lengthSpan" v-for="(value, key) in item1.fileTotalTime" :key="key"></span><span class="voiceSpan" :class="{voicePlay: item1.play}">
									</span>
									<span class="timeVoice">{{item1.fileTotalTime}}''</span>
								</li>
							</ul>
						</p>
					</div>
				</div>
			</div>
			<!-- <div v-if="recordDetail.isReturnVisit" class="chatListCase">
				<div class="zhuiWen">
					<div>回访:
						<span></span>
					</div>
					<div class="zhuiContent">{{recordDetail.returnVisit}}</div>
				</div>
			</div> -->
			<div v-if="recordDetail.refundState === 'return'" class="returnVisit">
				<p v-if="refundReason" class="conditionP">
					退单原因：{{refundReason}}
				</p>
				<p v-if="recordDetail.refundRemarks" class="conditionP">
					退单描述：{{recordDetail.refundRemarks}}
				</p>
				<p v-if="recordDetail.recommendDoctorID!=0"  class="conditionP red-hint">推荐医生：</p>
				<router-link v-if="recordDetail.recommendDoctorID!=0"  class="recommendDoctor" tag="div" :to="{path: '/look-detail', query: {id: recordDetail.recommendDoctorID}}">
					<div class="recommendImg">
						<img :src="recordDetail.recommendDoctorImage" />
					</div>
					<div class="recommendText">{{recordDetail.recommendDoctorName}}</div>
				</router-link >
			</div>
			
			
			
			<!-- <div v-if="show" class="illness-issue">
				<router-link to="/illnessCase" class="okBtn">
					<mt-button type="primary" class="mint-button--large">发布病例</mt-button>
				</router-link>
			</div> -->
			<!-- <div @click="uplodAudio">
				语音发送
			</div> -->
        </div>
		<!-- <div v-show="videoShow" class="videoLook">
            <div class="videoBox">
                <video id="videoSelf" controls :src="videoSrc" autoplay muted></video>
            </div>
			<div class="videoBtn">
				<mt-button @click="videoClick(false)" type="primary" class="mint-button--large">关闭</mt-button>
			</div>
        </div> -->
		<div v-show="false">
			<audio ref="audioCase" id="audioCase" src=""></audio>
		</div>
		
    </div>
</template>

<script>
let moment = require('moment');
export default {
    data () {
        return {
			id: 0,
			returnShow: true,
			videoSrc: '',
			videoShow: false,
			replys: [],
			consultationfile: [],
			recordDetail: {},
			sex: '',
			refundReason: '',
			compressHead: '?imageView2/q/20'
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
		consultationDetail () { // 获取咨询详情
			const id = this.id
            this.instance.consultationDetailD({
				id
			})
                .then((response) => {
					let recordDetail = response.data.result.item
					this.recordDetail = recordDetail
					this.replys = recordDetail.replys
					this.returnVisit = recordDetail.returnVisit
					this.consultationfile = recordDetail.consultationfile
					this.recordDetail.iiInessDescription = this.recordDetail.iiInessDescription.replace(/\n/g,"<br/>")
					const sex = recordDetail.sex
					const sexStatus = ['', '男', '女']
					this.sex = sexStatus[sex]
					if (this.replys.length > 0) {
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
					}
					this.$previewRefresh()
					if (recordDetail.refundReason) {
						this.refundReason = JSON.parse(recordDetail.refundReason).LabelName
					}
                })
                .catch((error) => {
                }) 
		}
	},
	mounted () {
		this.consultationDetail()
	},
	created () {
		this.id = parseInt(this.$route.query.id)
	}
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.orderLook {
	padding: 42px 0 0;
	.videoLook {
        position: fixed;
        left: 0;
        right: 0;
        bottom: 0;
        top: 0;
        z-index: 10000000;
        background: #333;
        .videoBox {
            width: 100%;
            video {
                width: 100%;
            }
		}
		.videoBtn {
			padding: 20px 15px;
		}
    }
	.recommendDoctor {
		display: flex;
		.recommendImg {
			width: 40px;
			height: 40px;
			img {
				width: 100%;
				height: 100%;
				border-radius: 5px;
			}
		}
		.recommendText {
			flex: 1;
			line-height: 40px;
			padding-left: 10px;
		}
	}
	.callTimeList{font-size: $font-l;}
	.condition {margin: 10px 0 0; background: $color-wfont; font-size: $font-t; color: #999;}
	.conditionP {padding-bottom: 10px;}
	.condition span {color: $color-afont;}
	.conditionP1 span {font-size: $font-m; background: $default-color; color: #fff; float: right; display: inline-block; width: 60px; height: 20px;text-align: center;line-height: 20px;border-radius: 3px;}
	.condition .paList li {border-top: 1px dashed #eee; width: 100%; float: left; padding: 10px 0; border-bottom: none;}
	.paList .illCase {
		line-height: 20px;
		padding: 0 10px; 
		display: block;
	}
	.condition .paList .shortLi {width: 50%;}
	.returnVisit {background-color: #fff; padding: 10px; font-size: $font-l;}
	
	.condition .paList .border-none {border: none;}
	img {border-radius: 3px;}
	.zhuiWen { padding: 10px 15px; background: #fff;}
	.zhuiWen div {padding: 10px 0; font-size: $font-t; color: $color-bfont;}
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
        background: url(../../assets/image/audioBg1.png) no-repeat;
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
	.weTime div {padding: 3px 5px; color: #ccc; font-size: $font-l; border-radius: 3px;}
	.weChat p {font-size: $font-t; color: #333;}
	.weTitle {background: #eee;display: inline-block; float: right; padding: 10px; margin-right: 50px; position: relative; border-radius: 5px;}
	.weTitle img {width: 80px;}
	.weTitle .avatar {position: absolute; right: -50px; top: 0; width: 40px; height: 40px; border-radius: 5px; padding: 0;}
	.weTitle p {width: 100%;word-break: break-all;}
}

</style>