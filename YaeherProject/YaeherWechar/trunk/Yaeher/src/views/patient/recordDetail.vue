<template>
    <div class="record-detail padding-top">
		<mt-header fixed title="咨询详情">
            <a v-if="returnShow" @click="goList" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
            <ul class="mui-table-view">
				<li v-if="recordDetail.consulationStatusCode === 'processing'" class="mui-table-view-cell answer-me red-hint">
					咨询订单处理中，请稍候！
			    </li>
				<li class="mui-table-view-cell answer-me">
			        <router-link :to="{path: '/doctor-detail-patient', query: {id: recordDetail.doctorID}}"  class="mui-navigate-right">
			        	<div class="answer">
			        		<div class="answer1">
			        			为我解答
			        		</div>
			        		<div class="doctorAn">
			        			<img class="img" :src="recordDetail.userImage" />
			        			<span>{{recordDetail.doctorName}}</span>
                                <i class="mint-cell-allow-right"></i>
			        		</div>
			        	</div>
			        </router-link>
			    </li>
			</ul>
			<div class="condition">
				<p v-if="recordDetail.consultType === 'ImageText'" class="conditionP payCase">图文咨询 
					<!-- <mt-button v-if="recordDetail.consulationStatusCode === 'unpaid'" @click="toPayFor" class="goPay" type="primary">去支付</mt-button> -->
				</p>
				<p v-if="recordDetail.consultType === 'Phone'" class="conditionP payCase">电话咨询
					<!-- <mt-button v-if="recordDetail.consulationStatusCode === 'unpaid'" @click="toPayFor" class="goPay" type="primary">去支付</mt-button> -->
				</p>
				<ul class="paList">
					<li>
						姓名：<span>{{recordDetail.patientName}}</span>
					</li>
					<li>
						性别：<span>{{sex}}</span>
					</li>
					<li>
						年龄：<span>{{recordDetail.age}}</span>
					</li>
					<li>
						病种：<span>{{recordDetail.iiInessType}}</span>
					</li>
					<li>
						所在地：<span>{{recordDetail.patientCity}}</span>
					</li>
					<li>
						病情描述：
					</li>
					<li>
						<p v-html="recordDetail.iiInessDescription" class="font-describe"></p>
						<ul class="imgCaseList">
							<li v-if="item.mediatype === 'image'" class="showImg" v-for="(item, index) in consultationfile" :key="index" >
								<!-- <img :src="item.message" /> -->
								<!-- <img :src="`${item.message.replace(/cos.ap-guangzhou/g, 'picgz')}${compressHead}`" :large="item.message" preview="patient1" preview-text=""> -->
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
							<li class=""  v-if="item1.mediaType === 'image'" v-for="(item1, index1) in item.consultationFile" :key="index1">
								<!-- <img v-gallery:groupName :src="item1.fileUrl" /> -->
								<!-- <img :src="`${item1.fileUrl.replace(/cos.ap-guangzhou/g, 'picgz')}${compressHead}`" :large="item1.fileUrl" :preview="index" preview-text=""> -->
								<img :src="`${item1.fileUrl.replace(/cos.ap-guangzhou/g, 'picgz')}${compressHead}`" @click="readImgInfo(item.consultationFile, item1.fileUrl, 'fileUrl')" >
							</li>
							<!-- <li class="" v-if="item1.mediaType === 'video'" v-for="(item1, index1) in item.consultationFile" :key="index1">
								<img @click="videoClick(true, item1.message)" src="../../assets/image/videoPlay.jpg" />
							</li> -->
						</ul>
					</div>
				</div>
				<div v-if="item.replyType==='answer'"  class="weChat">
					<div class="weTime">
						<div>{{item.createdOn}}</div>
					</div>
					<div v-if="item.answerType === 'Phone'" class="weTitle">
						<router-link :to="{path: '/doctor-detail-patient', query: {id: recordDetail.doctorID}}">
							<img class="avatar" :src="recordDetail.userImage" />
						</router-link>
						<p>
							{{recordDetail.doctorName}}在 {{item.createdOn}} 通过电话回复给您！
						</p>
					</div>
					<div v-if="item.answerType === 'Message'" class="weTitle">
						<router-link :to="{path: '/doctor-detail-patient', query: {id: recordDetail.doctorID}}">
							<img class="avatar" :src="recordDetail.userImage" />
						</router-link>
						<p v-html="item.message">
						</p>
						<p>
							<ul class="replaysBox">
								<li class=""  v-if="item1.mediaType === 'image'" v-for="(item1, index1) in item.consultationFile" :key="index1">
									<!-- <img v-gallery:groupName :src="item1.fileUrl" /> -->
									<!-- <img :src="item1.fileUrl" :preview="index" preview-text=""> -->
									<!-- <img :src="`${item1.fileUrl.replace(/cos.ap-guangzhou/g, 'picgz')}${compressHead}`" :large="item1.fileUrl" :preview="index" preview-text=""> -->
									<img :src="`${item1.fileUrl.replace(/cos.ap-guangzhou/g, 'picgz')}${compressHead}`" @click="readImgInfo(item.consultationFile, item1.fileUrl, 'fileUrl')" >
								</li>
								<!-- <li class="" v-if="item1.mediaType === 'video'" v-for="(item1, index1) in item.consultationFile" :key="index1">
									<img @click="videoClick(true, item.fileUrl)" src="../../assets/image/videoPlay.jpg" />
								</li> -->
								<li class="voiceLi" @click="playAudio(index, item1.fileUrl, item1.fileTotalTime, item1.play)" v-if="item1.mediaType === 'voice'" v-for="(item1, index1) in item.consultationFile" :key="index1">
									<span class="lengthSpan" v-for="(value, key) in item1.fileTotalTime" :key="key"></span><span class="voiceSpan" :class="{voicePlay: item1.play}">
									</span>
									<span class="timeVoice">{{item1.fileTotalTime}}''</span>
								</li>
							</ul>
						</p>
						
					</div>
					<div class="warmPrompt">温馨提示：网络咨询意见仅供参考，不能替代临床诊疗。</div>
				</div>
			</div>
			<!-- <div v-if="recordDetail.isReturnVisit" class="returnVisit">
				<p>
					回访：
					<span class="text-right"></span>
				</p>
				<p class="font-describe">
					{{returnVisit}}
				</p>
			</div> -->
			<div v-if="recordDetail.refundState === 'return'" class="returnVisit">
				<p v-if="refundReason" class="conditionP">
					退单原因：{{refundReason}}
				</p>
				<p v-if="recordDetail.refundRemarks" class="conditionP">
					退单描述：{{recordDetail.refundRemarks}}
				</p>
				<p v-if="recordDetail.recommendDoctorID!=0" class="conditionP red-hint">推荐医生：</p>
				<router-link v-if="recordDetail.recommendDoctorID!=0" class="recommendDoctor" tag="div" :to="{path: '/doctor-detail-patient', query: {id: recordDetail.recommendDoctorID}}">
					<div class="recommendImg">
						<img :src="recordDetail.recommendDoctorImage" />
					</div>
					<div class="recommendText red-hint">{{recordDetail.recommendDoctorName}}</div>
				</router-link >
			</div>	
			<div class="notice" v-if="show && recordDetail.consultType != 'Phone'">
			<!-- <div class="notice"> -->
				<p class="conditionP border-bottom">
					追问
				</p>
				<div class="mui-input-row">
                    <mt-field label="追问" @input = "descInput" placeholder="填写追问信息" type="textarea" rows="5" :attr="{ maxlength: maxReplyLength }" v-model="repayIllnessDescription"></mt-field>
					<span class="hint-font">可输入{{remark}}字</span>
				</div>
				<!-- <div class="upload-row">
					<label>图片\视频上传 </label>
					<div class="uploadRow">
						<ul class="uploadImgList">
							<li class="imgListCase" v-for="(item, index) in imgList" :key="index">
								<span @click="deleteImg(index)" class="deleteBtn">X</span>
								<img v-gallery:groupName v-if="item.mediaType === 'image'" :src="item.src" />
								<video v-if="item.mediaType != 'image'" controls :src="item.src" autoplay="none"></video>
							</li>
						</ul>
						<div class="uploadImg">
							<label class="uploadImgCase">
								图片上传：
								<input style="margin: 10px 0;" multiple accept="image/*" type="file" v-on:change="uploadImgOss($event, 'image')"/>
							</label>    
							<label class="uploadVideoCase">
								视频上传：
								<input accept="video/*" type="file" v-on:change="uploadImgOss($event, 'video')"/>
							</label>
						</div>
					</div>
				</div> -->
				<div class="upload-row">
					<label class="uploadBox">
								图片上传
						<input ref="imgFile" accept="image/*" type="file" v-on:change="uploadImgOss($event, 'image')"/>
					</label>   
					<div class="uploadRow">
						<ul class="uploadImgList">
							<li class="imgListCase"  v-if="item.mediaType === 'image'&& !item.isDelete"  v-for="(item, index) in imgList" :key="index">
								<span v-if="item.mediaType === 'image'" @click="deleteImg(index)" class="deleteBtn">X</span>
								<!-- <img v-gallery:groupName v-if="item.mediaType === 'image'" :src="item.src" /> -->
								<mt-progress v-if="!item.upFlag" :value="item.progress" :bar-height="1">
									<div slot="end">{{item.progress}}%</div>
								</mt-progress>
								<div class="showImgCase">
									<!-- <img :src="`${item.src.replace(/cos.ap-guangzhou/g, 'picgz')}${compressHead}`" :large="item.src" preview="patient2" preview-text=""> -->
									<img :src="`${item.src.replace(/cos.ap-guangzhou/g, 'picgz')}${compressHead}`" @click="readImgInfo1(imgList, item.src, 'src')" >
								</div>
							</li>
						</ul>
					</div>
				</div>
				<!-- <div class="upload-row">
					<label class="uploadBox">
								上传视频
						<input accept="video/*" type="file" v-on:change="uploadImgOss($event, 'video')"/>
					</label>  
					<div class="uploadRow">
						<ul class="uploadImgList">
							<li class="imgListCase"  v-if="item.mediaType != 'image'"  v-for="(item, index) in imgList" :key="index">
								<span @click="deleteImg(index)" class="deleteBtn">X</span>
								<img @click="videoClick(true, item.src)" src="../../assets/image/videoPlay.jpg" />
							</li>
						</ul>
					</div>
				</div> -->
				<div>
					<div class="href" @click="consultationReply">提交追问</div>
				</div>
			</div>	
			<!-- <div class="notice" v-if="show1">
				<p class="conditionP border-bottom">
					回访
				</p>
				<div class="mui-input-row">
                    <mt-field label="回访" @input = "descInput1" placeholder="填写回访信息" type="textarea" rows="5" :attr="{ maxlength: maxReplyLength }" v-model="returnVisit"></mt-field>
					<span class="hint-font">可输入{{remark1}}字</span>
				</div>
				<div>
					<div class="href" @click="applyreturnVisit">提交回访</div>
				</div>
			</div> -->
				
			<div class="notice">
				<div v-if="recordDetail.hasInquiryTimes!=0 && recordDetail.consultType != 'Phone' && recordDetail.consulationStatusCode != 'unpaid' && recordDetail.consulationStatusCode != 'created'" class="timeHint">
					<span class="inquiryTimes">剩余追问次数:{{recordDetail.hasInquiryTimes}}<span>，如有需要请在{{recordDetail.inquiryTimesMsg}}内追问</span>
					</span>
				</div>
				<div class="btn-case">
					<span v-if="recordDetail.canDelete" @click="deleteConsultations" class="btn">删除</span>
					<router-link v-if="recordDetail.consulationStatusCode === 'created' || recordDetail.consulationStatusCode === 'unpaid'" :to="{path: '/consultation', query: {id: serviceMoneyListID, orderId: id, serviceType: this.consultType}}" class="btn">修改</router-link>
					<span v-if="recordDetail.canReplys && recordDetail.consultType != 'Phone'" class="btn" @click="btnClick(false)">追问</span>	
					<!-- <span v-if="!recordDetail.isReturnVisit && recordDetail.consulationStatusCode ==='success'" class="btn" @click="btnClick(true)">回访</span>					 -->
					<router-link v-if="recordDetail.canEvaluation" :to="{path: '/evaluate', query: {id}}" style="margin-top: 10px;" class="btn">评价</router-link>					
					<router-link v-if="recordDetail.canhargeback" :to="{path: '/chargeback', query: {id}}" class="btn">退单</router-link>
				</div>
				<div>
					<p v-if="recordDetail.isEvaluate" class="agin-evaluate">
						<span style="color: #FF0000; padding-right: 10px;"></span>您已提交评价，评分{{recordDetail.evaluateLevel}}星，点击
						<router-link :to="{path: '/evaluate-detail', query: {id: evaluationId}}">查看详情</router-link>
					</p>
					<router-link v-if="recordDetail.isEvaluate" :to="{path: '/doctor-detail-patient', query: {id: doctorId}}"   class="href">再次咨询</router-link>
				</div>
				<div v-if="recordDetail.consulationStatusCode === 'unpaid'" class="payBox">
					<mt-button @click="toPayFor" type="primary" class="mint-button--large">提交并支付</mt-button>
				</div>
			</div>
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
import { Toast, MessageBox } from 'mint-ui'
import { timestamp, verifyPrice, dataURLtoBlob, fontVery } from 'assets/js/common.js'
import wx from 'weixin-js-sdk'
import Exif from 'exif-js'
let moment = require('moment');
export default {
    data () {
        return {
			id: 0,
			doctorId: 0,
			serviceMoneyListID: 0,
			videoSrc: '',
			videoShow: false,
			returnShow: true,
			replys: [],
			replyType: [],
			replyState: [],
			recordDetail: {},
			consultationfile: [],
			repayIllnessDescription: '',
			refundReason: '',
			returnVisit: '',
			remark: 300,
			remark1: 300,
			maxReplyLength: 300,
			evaluationId: 0,
			sp_billno: '',
			show: false,
			show1: false,
			showFlag: 1,
			sex: '',
			imgList: [],
            imgSize: 8,
            imgCount: 9,
            videoSize: 50,
			videoCount: 3,
			clickFlag: true,
			clickTag: true,
			payClick: true,
			allClick: true,
			fileFolder: '',
            fileFolder1: '',
			bucket: '',
			fileFolder: '',
			consultNumber: '',
			imgUrlHead: '',
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
		readImgInfo1(listImg, value, str) {
            let imgList = []
            for (var i = 0; i < listImg.length; i ++) {
                if (!listImg[i].isDelete) {
                    let obj = ''
                    if (str === 'src') {
                        obj = listImg[i].src
                    } 
                    if (str === 'message') {
                        obj = listImg[i].message
                    }
                    imgList.push(obj)
                }
            }
            console.log(imgList)
            WeixinJSBridge.invoke("imagePreview", {
                current: value,
                urls: imgList
            });
        },
		goList() {
			let _this = this
			if(this.recordDetail.consulationStatusCode === 'unpaid'){
				MessageBox.confirm('', {
					message: '您的咨询还未完成支付，医生还看不到您提交的咨询，请您尽快完成支付', 
					title: '温馨提示', 
					confirmButtonText: '继续支付', 
					cancelButtonText: '确认离开' 
				}).then(action => {

				},function(){
					console.log('取消')
					_this.$router.push({ path: '/user-record'})
				})	
			} else {
				_this.$router.push({ path: '/user-record'})
			}	
			
		},
		videoClick(flag, video) {
            this.videoShow = flag
            if (flag) {
				this.videoSrc = video
				let videoS = document.getElementById("videoSelf")
				console.log(videoS)
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
		descInput() {
            let txtVal = this.repayIllnessDescription.length;
            this.remark = this.maxReplyLength - txtVal;
		},
		descInput1() {
            let txtVal = this.returnVisit.length;
            this.remark1 = this.maxReplyLength - txtVal;
        },
		btnClick (flag) {
			if (flag) {
				this.show1 = !this.show1
			} else {
				this.show = !this.show
			}
			
		},
		consultationDetail1 () { // 获取咨询详情
			const id = this.id
			const consultNumber = this.consultNumber
			let params = {}
			if (id === 0 ) {
				params = {
					consultNumber
				}
			} else {
				params = {
					id
				}
			}
            this.instance.consultationDetail(
				params
            )
                .then((response) => {
					let recordDetail = response.data.result.item
					if (id === 0 ) {
						this.id = recordDetail.id
					} else {
						this.consultNumber = recordDetail.consultNumber
					} 
					this.consultType = recordDetail.consultType
					console.log(this.consultType)
					this.recordDetail = recordDetail
					this.replys = recordDetail.replys
					this.evaluationId = recordDetail.evaluationId
					this.returnVisit = recordDetail.returnVisit
					this.consultationfile = recordDetail.consultationfile
					this.doctorId = recordDetail.doctorID
					this.serviceMoneyListID = recordDetail.serviceMoneyListID
					console.log(this.serviceMoneyListID)
					this.recordDetail.iiInessDescription = this.recordDetail.iiInessDescription.replace(/\n/g,"<br/>")
					window.sessionStorage.setItem('doctorId', this.doctorId)
					window.sessionStorage.setItem('doctorName', recordDetail.doctorName)
					window.sessionStorage.setItem('doctorAvatar', recordDetail.userImage)
					const sex = this.recordDetail.sex
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
						console.log(this.replys)
					}
					this.$previewRefresh()
					if (recordDetail.refundReason) {
						this.refundReason = JSON.parse(recordDetail.refundReason).LabelName
					}
					
                })
                .catch((error) => {
                }) 
		},
		deleteConsultations () { // 删除咨询
			MessageBox.confirm('确认删除该咨询？').then(action => {
				const id = this.id
				this.instance.deleteConsultation({
					id
				})
					.then((response) => {
						this.$router.push({ path: '/user-record'})
					})
					.catch((error) => {
					}) 
            },function(){
                console.log('取消了');
            })
			
		},
		replyParameter () { // 提交追问参数
            this.instance.consultationReplyParameter({
            })
                .then((response) => {
					let replyList = response.data.result.item
					this.replyState = replyList.replyState
					this.replyType = replyList.replyType
					this.maxReplyLength = replyList.maxReplyLength
					this.remark = replyList.maxReplyLength
					
                })
                .catch((error) => {
                }) 
		},
		// applyreturnVisit () { // 提交回访
			
		// 	const id = this.id
		// 	const returnVisit = this.returnVisit

		// 	if (!returnVisit) {
		// 		Toast('请填写回访信息')
        //         return 
		// 	}

		// 	if(!this.clickTag) {
        //         MessageBox('提示信息', '请不要重复提交')
        //         return
		// 	}
		// 	this.clickTag = false
			
		// 	this.instance.createReturnVisit({
		// 		id,
		// 		returnVisit
        //     })
        //         .then((response) => {
		// 			if(response.data.result.code === 200){
		// 				Toast('回访成功')
		// 				location.reload()
		// 			}
        //         })
        //         .catch((error) => {
		// 			this.clickTag = true
        //         }) 
		// },
		consultationReply () { // 提交追问
            if (!this.allClick) {
                Toast('图片正在上传中，请稍候！')
                return
            }
			let _this = this
			let imgList = this.imgList
			let attach = new Array()
            // for (var i = 0; i < this.imgList.length; i ++) {
            //     let obj = {
			// 		id: 0,
            //         filename: this.imgList[i].filename,
            //         mediaType: this.imgList[i].mediaType,
			// 		fileSize: this.imgList[i].fileSize
            //     }
            //     attach.push(obj)
			// }
			for (var i = 0; i < this.imgList.length; i ++) {
				if (!this.imgList[i].isDelete) {
					let obj = {
						id: 0,
						filename: this.imgList[i].filename,
						mediaType: this.imgList[i].mediaType,
						fileSize: this.imgList[i].fileSize
					}
					attach.push(obj)
				}
			}
			console.log('attch',attach)
			
			const replyStateCode = this.replyState[1].code
			const replyTypeCode = this.replyType[1].code
			const repayIllnessDescription = this.repayIllnessDescription
			const consultID = this.id
			if (!repayIllnessDescription) {
				Toast('追问信息不能为空')
				return
			}
			// let fontFlag = fontVery(repayIllnessDescription)
            // if (!fontFlag) return
			if(!this.clickTag) {
                MessageBox('提示信息', '请不要重复提交')
                return
			}
			this.$indicator.open({
                text: '提交中请稍候',
                spinnerType: 'fading-circle'
			})
			this.clickTag = false
			_this.questionClosely()
            // if (imgList.length === 0) {
			// 	_this.questionClosely()
            // } else {
			// 	console.log('传了文件')
            //     for (var j = 0; j < imgList.length; j ++) {
			// 		this.uploadCos(imgList[j].storeAs, imgList[j].file1, imgList[j].mediaType, j)
			// 	}
            // }
			
            
		},
		questionClosely() { // 追问
			let _this = this
			let imgList = this.imgList
			let attach = new Array()
            for (var i = 0; i < this.imgList.length; i ++) {
				if (!this.imgList[i].isDelete) {
					let obj = {
						id: 0,
						filename: this.imgList[i].filename,
						mediaType: this.imgList[i].mediaType,
						fileSize: this.imgList[i].fileSize
					}
					attach.push(obj)
				}
			}
			console.log('attch',attach)
			
			const replyStateCode = this.replyState[1].code
			const replyTypeCode = this.replyType[1].code
			const repayIllnessDescription = this.repayIllnessDescription
			const consultID = this.id
			if(!repayIllnessDescription && attach.length === 0) {
				Toast('追问内容不能为空')
				return
			}
			this.instance.createConsultationReply({
				consultID,
				replyStateCode,
				replyTypeCode,
				repayIllnessDescription,
				attach
            })
                .then((response) => {
					_this.$indicator.close()
                    Toast('追问成功')
					setTimeout(function() {
						_this.$router.push({ path: '/user-record'})
					}, 500)
                })
                .catch((error) => {
					_this.$indicator.close()
					Toast('提交追问失败')
					_this.clickTag = true
                }) 
		},
        uploadImgOss(evt, index2) { // 选择图片
            let file = evt.target
            let _this = this
			let fileList = file.files
			let src = ''
			if(fileList.length != 0) {
                 this.$indicator.open({
                    text: '图片上传中，请稍候',
                    spinnerType: 'fading-circle'
                })
            }
			 // 添加多个图片
            // for(let i = 0; i < fileList.length; i++) {
                let imgType = /^image\//
                let videoType = /^video\//
                let file1 = file.files[0]
				let fileName = fileList[0].name
                let suffix = fileName.split('.')
                suffix = suffix.pop()

                if (suffix === 'webp') {
                    Toast('暂不支持webp格式的图片')
                    return
                }
                let nameTime = timestamp()  
				let filename = `wx-${nameTime}.${suffix}`
				
				let fileSize = file.files[0].size
				
				let Orientation
				//去获取拍照时的信息，解决拍出来的照片旋转问题
				Exif.getData(file1, function(){
					Exif.getAllTags(this)
					// alert(Exif.pretty(this))
					// alert(Exif.getTag(this, 'Orientation'))
					
					Orientation = Exif.getTag(this, 'Orientation')
				});

				let storeAs = ''
				let mediaType = index2
                if (imgType.test(file1.type) && index2 === 'image') {
					console.log('上传了图片')
					storeAs = `${this.fileFolder}/${filename}` 
                    if (fileSize >= _this.imgSize*1024*1024) {
						setTimeout(function(){
                            _this.$indicator.close()
                        }, 200)
						Toast(`图片不能超过${_this.imgSize}M`)
                        return
                    }
                } else if (videoType.test(file1.type) && index2 === 'video') {
					console.log('上传了视频')
					storeAs = `${this.fileFolder1}/${filename}` 
                    if (fileSize >= _this.videoSize*1024*1024) {
                        Toast(`视频不能超过${_this.videoSize}M`)
                        return
                    }
                } else {
                    console.log('请上传正确文件')
                    return
				}
				
                let reader = new FileReader()
                reader.readAsDataURL(file1)
                reader.onload = function () {
					
					let imgSum = 0
					let videoSum = 0
					let imgSum1 = 0
					for (let n = 0; n < _this.imgList.length; n++) {
						if(_this.imgList[n].mediaType === 'image') {
							imgSum = imgSum + 1
							if (_this.imgList[n].isDelete === false) {
                                    imgSum1 = imgSum1 + 1
                                }
						}else {
							videoSum = videoSum + 1
						}
					}

					let obj = {
						fileSize,
						file1,
						filename,
						storeAs,
						mediaType,
						src,
						imgSum,
						isDelete: false,
						upFlag: false,
						progress: 0
					}
					_this.imgSum = imgSum
					if(obj.mediaType === 'image') {
						if (imgSum < _this.imgCount) {
							let result = this.result;
							let img = new Image();
							img.src = result;
							img.onload = function () {
								if (!Orientation || Orientation === 1) {
									// obj.src = result;
								} else {
									let src1 = _this.compress(img, Orientation);
									obj.file1 = dataURLtoBlob(src1)
									file1 = dataURLtoBlob(src1)
								}
								console.log(file1)
                                _this.imgList.push(obj)
							}
						} else {
							_this.$indicator.close()
							Toast(`图片最多上传${_this.imgCount}个`)
						}
					} else {
						if (videoSum < _this.videoCount) {
							_this.imgList.push(obj)
						} else {
							Toast(`视频最多上传${_this.videoCount}个`)
						}
					}
					setTimeout(function(){
						_this.uploadCos(storeAs, file1, mediaType, imgSum, filename)
					}, 500)
                }
			// }
        },
        uploadCos (key, body, type, index, filename, nub) { // 上传图片
			// 初始化实例
			this.allClick = false
            let _this = this
            var COS = require('cos-js-sdk-v5')
            var cos = new COS({
                SecretId: this.secretId,
                SecretKey: this.secretKey,
            })
            cos.putObject({
                Bucket: this.bucket, /* 必须 */
                Region: this.region,    /* 必须 */
                Key: key,              /* 必须 */
                Body: body, // 上传文件对象
                onProgress: function(progressData) {
					let progress = parseInt(progressData.percent*100)
					_this.imgList[index].progress = progress
                    console.log(progress)
                    
                }
            }, function(err, data) {
                console.log(err || data)
                if (err) {
					_this.$indicator.close()
					console.log(err)
                } else {
					console.log(data)
					_this.$indicator.close()
					let src = `${_this.imgUrlHead}${filename}`
					_this.imgList[index].src = src
                	_this.imgList[index].upFlag = true
					let flagAll = true
					for (let i = 0; i < _this.imgList.length; i++) {
						if (!_this.imgList[i].isDelete) {
							flagAll = flagAll && _this.imgList[i].upFlag
						}
						
					}
					console.log('end', flagAll)

					if (flagAll) {
                        _this.allClick = true
                    }
                }
            })
        },
        deleteImg (index) { // 删除图片
			// this.imgList.splice(index, 1)
			if (this.imgSum === this.imgList[index].imgSum) {
                this.$indicator.close()
                this.$refs.imgFile.value =''
            }
            this.imgList[index].isDelete = true
            let flagAll = true
            for (let i = 0; i < this.imgList.length; i++) {
                if (!this.imgList[i].isDelete) {
                    flagAll = flagAll && this.imgList[i].upFlag
                }
            }
            if (flagAll) {
                this.allClick = true
            }    
            console.log(flagAll)
        },
        getTypeParams () { // 获取type参数
            this.instance.tencentCosAccessTokenType({
            })
                .then((response) => {
					let paramsObject = response.data.result.item
                    this.mediaType = response.data.result.item.mediaType
					this.serviceType = response.data.result.item.type[1].code
					this.imgSize = paramsObject.consultationImagesize
                    this.imgCount = paramsObject.consultationImageCount
                    this.videoSize = paramsObject.videoSize
					this.videoCount = paramsObject.videoCount
					if (paramsObject.imageThumNail) {
                        this.compressHead = paramsObject.imageThumNail[0].value
                    }
                    this.getUploadParams(0)
					this.getUploadParams(1)
                })
                .catch((error) => {
                }) 
        },
        getUploadParams (index) { // 获取上传参数
            const serviceType = this.serviceType
			const mediaType = this.mediaType[index].code
			console.log(mediaType)
            this.instance.tencentCosAccessToken({
                mediaType,
                serviceType
            })
                .then((response) => {
				   const uploadParams = response.data.result.item
				   console.log(uploadParams)
				   this.bucket = uploadParams.bucket
				   if (index === 0) {
					   this.fileFolder = uploadParams.fileFolder
					   this.imgUrlHead = uploadParams.fileHeadName
					   console.log(this.fileFolder)
                   } else {
					   this.fileFolder1 = uploadParams.fileFolder
					   console.log(this.fileFolder1)
                   }
                   this.region = uploadParams.region
                   this.secretId = uploadParams.secretId
                   this.secretKey = uploadParams.secretKey
                })
                .catch((error) => {
                }) 
		},
		compress (img, Orientation) {
            let canvas = document.createElement("canvas");
            let ctx = canvas.getContext('2d');
            ctx.fillStyle = "#fff";
            ctx.fillRect(0, 0, canvas.width, canvas.height);
            if(Orientation != "" && Orientation != 1){
                switch(Orientation){
                    case 6://需要顺时针（向左）90度旋转
                        console.log(666666666)
                        this.rotateImg(img,'left',canvas);
                        
                        break;
                    case 8://需要逆时针（向右）90度旋转
                        console.log(8888888)
                        this.rotateImg(img,'right',canvas);
                        break;
                    case 3://需要180度旋转
                        console.log(3333333333)
                        this.rotateImg(img,'right',canvas);//转两次
                        this.rotateImg(img,'right',canvas);
                    break;
                }
            }   
            let ndata = canvas.toDataURL('image/jpeg', 1) 
            console.log('ndata', ndata)
            return ndata;
        },
        rotateImg (img, direction,canvas) {
            //最小与最大旋转方向，图片旋转4次后回到原方向    
            console.log('rotateImg')
            const min_step = 0;    
            const max_step = 3;      
            if (img == null)return;    
            //img的高度和宽度不能在img元素隐藏后获取，否则会出错    
            let height = img.height;    
            let width = img.width;      
            let step = 2;    
            if (step == null) {    
                step = min_step;    
            }    
            if (direction == 'right') {    
                step++;    
                //旋转到原位置，即超过最大值    
                step > max_step && (step = min_step);    
            } else {    
                step--;    
                step < min_step && (step = max_step);    
            }     
            //旋转角度以弧度值为参数    
            let degree = step * 90 * Math.PI / 180;    
            let ctx = canvas.getContext('2d');    
            console.log(step,'step')
            switch (step) {    
            case 0:    
                canvas.width = width;    
                canvas.height = height;    
                ctx.drawImage(img, 0, 0);    
                break;    
            case 1:    
                canvas.width = height;    
                canvas.height = width;    
                ctx.rotate(degree);    
                ctx.drawImage(img, 0, -height);    
                console.log('over')
                break;    
            case 2:    
                canvas.width = width;    
                canvas.height = height;    
                ctx.rotate(degree);    
                ctx.drawImage(img, -width, -height);    
                break;    
            case 3:    
                canvas.width = height;    
                canvas.height = width;    
                ctx.rotate(degree);    
                ctx.drawImage(img, -width, 0);    
                break;
            }    
		},
		submutOrder() { // 提交支付订单
			// const consultNumber = this.consultNumber
			// const sp_billno = this.sp_billno
			const serviceType = this.consultType
			Toast('咨询提交成功，医生看到后会尽快回复您，请耐心等待')
			this.$router.push({ path: '/patient-success' , query: {serviceType}})
			// this.$indicator.open({
            //     text: '提交中请稍候',
            //     spinnerType: 'fading-circle'
            // })
            // this.instance.consultationPaid({
            //     consultNumber,
            //     sp_billno
            // })
            //     .then((response) => {
			// 		this.$indicator.close()
            //         if(response.data.result.code === 200) {
			// 			Toast('咨询提交成功，医生看到后会尽快回复您，请耐心等待')
			// 			this.$router.push({ path: '/patient-success' , query: {serviceType}})
			// 		} else {
			// 			this.consultationDetail1()
			// 		}
					
                    
            //     })
            //     .catch((error) => {
			// 		this.$indicator.close()
			// 		this.consultationDetail1()
            //     })    
        },
		toPayFor() { // 去支付
			let _this = this
			if(!this.payClick) {
				Toast('请勿重复点击')
				return
			}
			this.payClick = false
			console.log('点了支付')
			let consultNumber = this.consultNumber
            this.instance.wXOAuthPay({
                consultNumber
            })
                .then((response) => {
                    let paramsObject = response.data.result.item
                    console.log(paramsObject)
                    let appId = paramsObject.appid
                    let timestamp = paramsObject.timeStamp
                    let nonceStr = paramsObject.nonceStr
                    let package1 = paramsObject.package
                    let signType = 'MD5'
                    let signature = paramsObject.paySign
                    let paySign = paramsObject.paySign
                    this.sp_billno = paramsObject.sp_billno
                    let obj = {
                        appId,
                        timestamp,
                        nonceStr,
                        package1,
                        signType,
                        signature,
                        paySign
                    }
                    wx.config({
                        debug: false,
                        appId,
                        timestamp,
                        nonceStr,
                        signature,
                        jsApiList: ['chooseWXPay']
                    })
                    wx.ready(function () {
                        wx.chooseWXPay({ // 微信支付
                            timestamp,
                            nonceStr,
                            'package': package1,
                            signType,
                            paySign,
                            success: function (res) {
								_this.submutOrder()
                            },
                            fail: function (err) {
                                console.log(err)
								_this.instance.wXOAuthPayProcessingRelease({
                                    consultNumber
                                })
                                    .then((response) => {
									   _this.payClick = true
									   _this.consultationDetail1()
                                    })
                                    .catch((error) => {
										_this.payClick = true
										_this.consultationDetail1()
                                    }) 
                            },
                            cancel: function (res) {
                                _this.instance.wXOAuthPayProcessingRelease({
                                    consultNumber
                                })
                                    .then((response) => {
									   _this.payClick = true
									   _this.consultationDetail1()
                                    })
                                    .catch((error) => {
										_this.payClick = true
										_this.consultationDetail1()
                                    }) 
                            }
                        })
                    })
                    wx.error(function (res) {
                        _this.payClick = true
                    })
                    
                })
                .catch((error) => {
                    _this.payClick = true
                }) 
		}
	},
	mounted () {
		this.consultationDetail1()
		this.replyParameter()
		this.getTypeParams()
	},
	created () {
		let id = this.$route.query.id
		if (id) {
			this.id = parseInt(id)
		} else {
			// this.consultNumber = this.$route.query.consultNumber
			this.consultNumber = window.sessionStorage.getItem('consultNumber')
			this.returnShow = false
		}
	}
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.record-detail {
	-webkit-user-select: text;
	-moz-user-select: text;
	-ms-user-select: text;
	user-select: text;
	.payBox {
		padding: 15px 0 10px;
	}
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
	.callTimeList{font-size: $font-l;}

	
	.replaysBox {
		// overflow: hidden;
		padding: 0 0 0 5px;
		background: #eee;
		position: relative;
		li {
			float: left;
			width: 80px;
			height: 80px;
			margin: 5px 5px 5px 0;
			padding: 0;
			position: relative;
			overflow: hidden;
			border-radius: 5px;
			img{
				position: absolute;
				top: 50%;
				left: 50%;
				display: block;
				height: 100%;
				transform:translate(-50%,-50%);
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
			overflow: visible;
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
			margin: 5px 9px 5px 0;
			padding: 0;
			position: relative;
		}
	}
	
	.answer-me {
		padding: 8px 0;
		margin-top: 10px;
		.answer {
			display: flex; 
			font-size:  $font-l; 
			margin: 0;
			.answer1 {
				width: 70px; 
				line-height: 20px;
			}
		}
	}

	.uploadBox {
        width: 100px;
        height: 38px;
        background: $default-color;
        position: relative;
        display: block;
        line-height: 38px;
        text-align: center;
        color: #fff;
        border-radius: 5px;
        input{
            visibility: hidden;
            position: absolute; display: block; width: 100%; height: 100%;
        }
    }

	img {border-radius: 3px;}
	.zhuiWen { padding: 10px 15px; background: #fff;}
	.zhuiWen div {padding: 10px 0; font-size: $font-l; color: $color-bfont;}
	.zhuiWen div span{float: right;}
	.zhuiWen .zhuiContent {color: $color-afont;}
	// .zhuiWen img { width: 100px; margin: 5px 5px 0 0;}
	.paList {padding: 0px;}
	.paList .imgCaseList {padding: 10px 0; overflow: hidden;}
	.paList .imgCaseList .showImg {float: left; width: 100px; 
	height: 100px; margin: 5px 9px 0px 0; padding: 0; overflow: hidden; border-top: none;}
	.paList .imgCaseList .showImg:nth-child(3n) {padding-right: 0px;}
	.paList .imgCaseList .showImg {
		position: relative;
		border-radius: 3px;
		img{
			// width: 100%;
			// height: 100%;
            position: absolute;
            top: 50%;
			left: 50%;
			height: 100%;
            display: block;
            // min-width: 100%;
            // min-height: 100%;
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
	.weChat {padding: 10px 10px 20px; background: #fff; overflow: hidden;position: relative;}
	.weTime {width: 100%; float: right; padding-bottom: 5px; overflow: hidden;}
	.weTime div {padding: 3px 5px 2px; background: #fff; color: $color-cfont; font-size: $font-m; border-radius: 3px; float: right; margin-right: 50px;}
	.weChat p {font-size: 17px; color: #333;}
	.warmPrompt{position: absolute; bottom: 5px; right: 55px; font-size: 9.5px; color: #ccc; line-height: 8.5px;}
	.weTitle {background: #eee;display: inline-block; float: right; padding: 10px; margin-right: 50px; position: relative; border-radius: 5px;}
	// .weTitle img {width: 80px;}
	.weTitle .avatar {position: absolute; right: -50px; top: 0; width: 40px; height: 40px; border-radius: 5px; padding: 0;}
	.weTitle p {width: 100%;word-break: break-all;}
	.upload-row {position: relative; background: $color-wfont; padding: 10px 10px;}
    .uploadImgList { padding: 10px 0; display: flex; flex-wrap:wrap;}
    .uploadImgList li {padding: 0; border-bottom: none;}
	.uploadImgList li .showImgCase{width: 100px; height: 100px; overflow: hidden; position: relative; border-radius: 3px;}
	.imgListCase {
        position: relative;
		width: 100px;
		height: 100px;
		padding: 0;
		margin: 5px 5px 0 5px;
        .deleteBtn {
            position: absolute;
            right: -6px;
            top: -6px;
            background-color: #ff0000;
            display: inline-block;
            color: #fff;
            width: 16px;
            height: 16px;
            font-size: 12px;
            border-radius: 8px;
            text-align: center;
            line-height: 17px;
            z-index: 3;
        }
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
    .uploadImg {padding-bottom: 15px; overflow: hidden;}
    .uploadImg > label {float: left; margin-right: 10px; position: relative;}
    .uploadImg input{visibility: hidden; position: absolute; display: block; width: 100%; height: 100%;}
    .uploadImgCase {width: 50px; height: 50px; background: url(../../assets/image/add-pic.png) no-repeat;background-size: 100%;}
    .uploadVideoCase {width: 50px; height: 50px; background: url(../../assets/image/add-pic.png) no-repeat;background-size: 100%;}
	.returnVisit {padding: 10px; background-color: #fff; font-size: $font-l;}
	.doctorAn {flex: 1; text-align: right; padding-right: 20px; position: relative;}
	.doctorAn .mint-cell-allow-right::after {right: 10px;}
	.doctorAn .img {width: 20px; height: 20px; border-radius: 3px;}
	.doctorAn img, .doctorAn span {display: inline-block; vertical-align: middle;}
	.doctorS img, .doctorS span {display: inline-block; vertical-align: middle;}
	.doctorS span {padding-left: 10px;}
	.condition {margin: 10px 0 0; padding: 10px; background: $color-wfont; font-size:  $font-l; color: #999;}
	.conditionP {padding-bottom: 10px;}
	.payCase {
		position: relative;
		.goPay {
			position: absolute;
			right: 0;
			bottom: 5px;
			font-size: 15px;
			height: 32px;
			padding: 0 10px 0 30px;
			background: #fe851b url(../../assets/image/payIcon.png) 8px 6px no-repeat;
			background-size: 20px;
		}
	}
	.border-bottom {border-bottom: 1px solid #eee;}
	.condition span {color: $color-afont;}
	.condition .paList li {border-bottom: none; border-top: 1px dashed #eee; padding: 10px 0;}
	.font-describe {line-height: 20px;padding: 0 10px; color: $color-afont}
	.mint-cell-title {display: none;}
	.doctorAnswer {padding: 10px 20px 10px 40px; background: $color-wfont; font-size: 12px; position: relative;}
	.doctorAnswer p {font-size:  $font-l; color: $color-afont;}
	.doctorTitle {display: flex; padding-bottom: 5px}
	.doctorTitle p {flex: 1; color: $color-afont;}
	.doctorAnswer img {position: absolute; top: 10px; left: 10px; width: 20px; height: 20px; border-radius: 5px;}
	.notice {background: $color-wfont; margin-top: 10px; padding: 10px;}
	.notice .href{width: 100%; height: 40px; color: $color-wfont; text-align: center; line-height: 40px; font-size: $font-xl; background: #35A3FF; display: block; border-radius: 5px;}
	.notice .mui-input-row {height: 160px; padding-bottom: 20px; position: relative;}
	.notice .mui-input-row .hint-font {position: absolute; bottom: 10px; right: 20px; color: #666;}
	.btn-case {padding: 5px 10px; text-align: right; overflow: hidden;}
	.inquiryTimes {float: left; font-size: $font-m; line-height: 29px;}
	.timeHint{overflow: hidden;}
	.notice .btn {display: inline; font-size: $font-l; padding: 5px 8px; border: 1px solid #ccc; line-height: 28px; margin-left: 10px; border-radius: 3px;}
	.agin-evaluate {padding: 10px 0; font-size: $font-l;}
	.agin-evaluate a {color: $default-color;}
	.href {margin-top: 10px;}
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
}


</style>