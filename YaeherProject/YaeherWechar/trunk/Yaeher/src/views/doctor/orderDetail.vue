<template>
    <div ref="srcollBox1" class="orderDetail padding-top">
		<router-view v-on:listenChildrenEvent="showChildrenData"/>
		<mt-header fixed title="咨询详情">
            <a v-if="returnShow" @click="$router.push({path: '/index-doctor'})" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
            <mt-button @click="operationShow"  slot="right">
                操作
            </mt-button>                    
        </mt-header>
        <div class="operation-case" v-show="operaShow">
            <ul>
                <router-link :to="{path: '/patient-order', query: {createdBy: recordDetail.createdBy}}" tag="li">咨询记录</router-link>
                <li v-if="!recordDetail.hasCollect" @click="collectOrder('收藏')">收藏</li>
				<li v-if="recordDetail.hasCollect" @click="collectOrder('取消收藏')">取消收藏</li>
                <router-link v-if="recordDetail.canhargeback" :to="{path: '/chargeback', query: {id, chargeType: 'doctor'}}" tag="li">退单</router-link>
				<li @click="closeText(true)">发送</li>
            </ul>
        </div> 
        
        <div @touchstart="moveContent($event)" ref="srcollBox" :class="{selectYes: vActive}" class="content p-content contCase">
			<div class="condition">
				<ul class="paList">
					<li class="border-none">
						<p v-if="recordDetail.consultType === 'ImageText'" class="conditionP1 ">图文咨询</p>
						<p v-if="recordDetail.consultType === 'Phone'" class="conditionP1 ">电话咨询
							<i class="callHint red-hint" v-if="recordDetail.consulationStatusCode !='return' && recordDetail.consulationStatusCode !='success'">（需至少向用户拨通一次电话）</i>
							<span v-if="recordDetail.consulationStatusCode !='return' && recordDetail.consulationStatusCode !='success'" @click="callPhone">拨打电话</span>
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
						病情描述：<span class="applyTime">{{recordDetail.createdOn}}</span>
					</li>
					<li>
						<!-- <p class="font-describe"></p> -->
						<p class="illCase" v-html="recordDetail.iiInessDescription" ></p>
						<ul class="imgCaseList">
							<li v-if="item.mediatype === 'image'" class="showImg" v-for="(item, index) in consultationfile" :key="index" >
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
				<!-- <div v-if="replys.length===0" class="moreHeight"></div> -->
			</div>
        	<div v-for="(item, index) in replys" :key="index" class="chatListCase">
				<div v-if="item.replyType==='inquiries'" class="zhuiWen">
					<div>追问:
						<span>{{item.createdOn}}</span>
						<p></p>
						<p v-html="item.message" class="zhuiContent"></p>
						<ul class="replaysBox replaysBox1">
							<li  v-if="item1.mediaType === 'image'" v-for="(item1, index1) in item.consultationFile" :key="index1">
								<!-- <img :src="`${item1.fileUrl.replace(/cos.ap-guangzhou/g, 'picgz')}${compressHead}`" :large="item1.fileUrl" :preview="index" preview-text=""> -->
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
						<a>
							<img class="avatar" :src="recordDetail.userImage" />
						</a>
						<p>
							您在 {{item.createdOn}} 进行了电话回复
						</p>
					</div>
					<div v-if="item.answerType === 'Message'" class="weTitle">
						<a>
							<img class="avatar" :src="recordDetail.userImage" />
						</a>
						<p v-html="item.message">
							<!-- {{item.message}} -->
						</p>
						<!-- <p> -->
							<ul class="replaysBox">
								<li class=""  v-if="item1.mediaType === 'image'" v-for="(item1, index1) in item.consultationFile" :key="index1">
									<!-- <img :src="`${item1.fileUrl.replace(/cos.ap-guangzhou/g, 'picgz')}${compressHead}`" :large="item1.fileUrl" :preview="index" preview-text=""> -->
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
						<!-- </p> -->
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
			<div ref="heightAdd" class="heightAdd"></div>
        </div>
		<div :class="{focusAbsoult: focusFlag}" v-if="recordDetail.consulationStatusCode !='return'"  ref="sendOut" class="sendOut">
			<div v-if="showBtn" class="upload-row">
				<!-- <label>图片\视频上传 </label> -->
				<div class="uploadRow">
					<!-- <ul class="uploadImgList">
						<li class="imgListCase" v-for="(item, index) in imgList" :key="index">
							<span @click="deleteImg(index)" class="deleteBtn">X</span>
							<img v-gallery:groupName v-if="item.mediaType === 'image'" :src="item.src" />
							<video v-if="item.mediaType != 'image'" :src="item.src" autoplay="none"></video>
						</li>
					</ul> -->
					<!-- <div class="uploadImg"> -->

					<!-- <label class="uploadBox">
						视频上传
						<input accept="video/*" type="file" v-on:change="uploadImgOss($event, 'video')"/>
					</label> -->
					<router-link tag="label" :to="{path: '/fast-replay', query: {id}}"  class="uploadBox">
						快捷回复
					</router-link> 
					<label class="uploadBox">
						图片上传
						<input ref="fileCase" multiple style="margin: 10px 0;" accept="image/*" type="file" v-on:change="uploadImgOss($event, 'image')"/>
					</label>    
					
					
					<!-- </div> -->
				</div>
			</div>
			<div v-show="!clickVioce" @click="voiceClick(true)" class="voiceChat replay03"></div>
			<div v-show="clickVioce" @click="voiceClick(false)" class="voiceChat replay02"></div>
			<div v-show="clickVioce" class="inputChat2" @touchstart="gtouchstart($event)" @touchend="gtouchend" @touchmove="gtouchmove($event)">
				按住录音
			</div>
			<div v-show="!clickVioce" class="inputChat" ref="inputChat">
				<!-- <input type="text" v-model="repayIllnessDescription" /> -->
				<textarea @focus="iptFocus($event)"  @blur="iptBlur"  rows="1" maxlength="5000"  ref="address" v-model="repayIllnessDescription"></textarea>
			</div>
			<div @click="showClick" class="sentImg replay04"></div>
			<div @click="closeText(true)" class="sendChat">发送</div>
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
		<div v-if="textShow" class="textShow">
			<!-- <div class="sendPreview">发送预览</div> -->
			<!-- <div v-html="repayShow" class="textContent"></div>	 -->
			<!-- <div class="textContent">
                <mt-field  @input = "descInput" label="简介" placeholder="填写快捷回复信息" type="textarea" rows="8" v-model="repayIllnessDescription" :attr="{ maxlength: 5000 }"></mt-field>
			</div> -->
			<div class="introduce-case">
                <mt-field label="" placeholder="填写回复内容" type="textarea" rows="10" v-model="repayIllnessDescription" :attr="{ maxlength: 5000 }"></mt-field>
                <!-- <p>可输入{{remark1}}字</p> -->
                
                <div class="okCase">
                    <mt-button @click="consultationReply(false)" type="primary" class="mint-button--large">确认发送</mt-button>
                </div>

                <div class="okCase">
                    <mt-button @click="closeText(false)" type="default" class="mint-button--large">返回</mt-button>
                </div>
            </div>	
			<!-- <ul class="navBar">
				<li @click="closeText(false)"><span class="">返回</span></li>
				<li @click="consultationReply(false)"><span class="green-hint">确认发送</span></li>
			</ul> -->
		</div>
    </div>
</template>

<script>
import { Toast, MessageBox } from 'mint-ui';
import { timestamp, dataURLtoBlob, hasClass1, fontVery } from 'assets/js/common.js'
import wx from 'weixin-js-sdk'
import Exif from 'exif-js'
// import Viewer from 'viewerjs';
// import 'viewerjs/dist/viewer.css';
let moment = require('moment');
let that = wx
let startTime
let endTime
let yStart = 0
let yEnd = 0
let voiceTime
let focusTime
let interval
export default {
    data () {
        return {
			id: 0,
			focusFlag: false,
			operaShow: false,
			vActive: true,
			show: false,
			clickVioce: false,
			returnShow: true,
			doctorId: 0,
			serviceMoneyListID: 0,
			videoSrc: '',
			videoShow: false,
			urlWx: '',
			replys: [],
			replyType: [],
			replyState: [],
			consultationfile: [],
			recordDetail: {},
			replayDemo: '',
			repayIllnessDescription: '',
			// remark1: 5000,
			remark: 300,
			maxReplyLength: 300,
			evaluationId: 0,
			show: false,
			showFlag: 1,
			sex: '',
			imgList: [],
			imgSize: 8,
            imgCount: 9,
            videoSize: 50,
            videoCount: 3,
            fileFolder: '',
            fileFolder1: '',
			bucket: '',
			fileFolder: '',
			caller: '',
			callee: '',
			showBtn: false,
			clickFlag: true,
			clickTag: true,
			consultNumber: '',
			refundReason: '',
			localId: 0,
            serverId: 0,
			breakOff: true,
			timeOver: true,
			textShow: false,
			repayShow: '',
			compressHead: '?imageView2/q/20',
			touchendClick: true
        }
    },
    methods: {
		// descInput() {
        //     let txtVal = this.repayIllnessDescription.length
        //     this.remark1 = 5000 - txtVal
		// },
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
		showChildrenData (data) { // 获取备注信息
			console.log(data)
			this.repayIllnessDescription = `${this.repayIllnessDescription}${data.content}`
      	},
		closeText (flag) {
			this.operaShow = false
			// if (flag) {
			// 	if (!this.repayIllnessDescription) {
			// 		Toast('回复内容不能为空')
			// 		return
			// 	}
			// }
			this.textShow = flag
			
			// this.repayShow = this.repayIllnessDescription.replace(/\n/g,"<br/>")
		},
		voiceClick(flag) {
			this.clickVioce = flag
			this.showBtn = false

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
		iptFocus(evt) {
			this.vActive = true
			this.inFocus = true
			this.focusFlag = true
			let _this = this
			var u = navigator.userAgent
			// var isAndroid = u.indexOf('Android') > -1 || u.indexOf('Linux') > -1;
			var isIOS = !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/); //ios终端
			// if (isAndroid) {
			// }
			if (isIOS) {
				let str = navigator.userAgent.toLowerCase();
				let ver = str.match(/cpu iphone os (.*?) like mac os/)[1].replace(/_/g,".");
				let oc = ver.split('.')[0];
				if(oc > 10){
					focusTime = setInterval(function () {
						_this.$refs.address.scrollIntoView()
					},300);
				}else{
					focusTime = setInterval(function () {
						document.body.scrollTop = document.body.scrollHeight
					},300);
				}
				
			}
		},
		iptBlur() {
			let _this = this
			this.vActive = true
			this.focusFlag = false
			// clearTimeout(focusTime)
			clearInterval(focusTime)
			
			_this.inFocus = false
			setTimeout(function () {
				if(_this.inFocus == false){
					_this.checkWxScroll()
				}	
			}, 200)
		},
		checkWxScroll(){
			var ua = navigator.userAgent.toLowerCase()
			var u = navigator.userAgent.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/)
			if(ua.match(/MicroMessenger/i) == 'micromessenger'&&!!u){
				this.temporaryRepair()
			}
		},
		temporaryRepair(){
			var currentPosition,timer
			var speed = 1
			timer=setInterval(function(){
				currentPosition=document.documentElement.scrollTop || document.body.scrollTop
				currentPosition-=speed
				window.scrollTo(0,0)
				clearInterval(timer)
			}, 1)
		},
        operationShow() {
            this.operaShow = !this.operaShow
		},
		showClick() {
			this.showBtn = !this.showBtn
		},
		moveContent () {
			
			let _this = this
			_this.$refs.address.blur()
			this.operaShow = false
			// _this.inFocus = false
			// setTimeout(function () {
			// 	if(_this.inFocus == false){
			// 		_this.checkWxScroll()
			// 	}	
			// }, 200)
		},
		gtouchstart(evt){ // 微信开始录音接口
			evt.preventDefault()
			this.vActive = false
			let _this = this
            this.$indicator.open({
                text: '正在录音,上滑取消，每次最长录制60S',
                spinnerType: 'double-bounce'
            })
			console.log('点了录音')
			yStart = evt.touches[0].pageY 
			// Toast('开始录音')
			
			that.startRecord()
			startTime = new Date().getTime()
			// 当用户长按60s自动中断录音
			voiceTime = setTimeout(() => {
				_this.timeOver = false
				_this.gtouchend()
            }, 57000)
			
            
		},
		longPress(){
			that.startRecord() // 微信开始录音接口
		},
		gtouchmove(evt){
			this.vActive = true
			yEnd = evt.touches[0].pageY
			if ( yStart - yEnd >= 30) {
				this.$indicator.close()
				this.breakOff = false
				
				Toast('录音已取消')
				that.stopRecord({ // 微信结束录音接口
					success: res => {
						this.localId = '';
						console.log('中断结束录音localId',this.localId)
					}
				})
			}
            
		},
		gtouchend(){
			if(!this.touchendClick) {
				return
			}
			this.touchendClick = false
			this.vActive = true
			let _this = this
            if(this.breakOff) {
				this.$indicator.close()
				clearTimeout(voiceTime)
				if (_this.timeOver) {
					_this.touchendClick = true
					endTime = new Date().getTime()
					let mistiming = endTime - startTime
					that.stopRecord({ // 微信结束录音接口
						success: res => {
							if (mistiming < 60000) {
								if (mistiming > 300) {
									Toast('录音结束')
									_this.localId = res.localId;
									console.log('正常结束录音成功了',_this.localId)
									_this.wxUpload()
								} else {
									_this.localId = ''
									Toast('录音时间太短')
									_this.$indicator.close()
								}
							} else {
								_this.localId = ''
								Toast('录音时间超过60S无效')
								_this.$indicator.close()
							}
						}
					})
				} else {
					that.stopRecord({ // 微信结束录音接口
						success: res => {
							Toast('录音60S完成')
							_this.timeOver = true
							_this.localId = res.localId;
							console.log('正常结束录音成功了',_this.localId)
							_this.wxUpload()
						}
					})
				}
				
            } else {
				this.breakOff = true
			}
            
        },
		wxUpload() { // 上传到微信服务器
			// Toast('上传录音')
			let _this = this
			that.uploadVoice({
				localId: this.localId, // 需要上传的音频的本地ID，由stopRecord接口获得
				isShowProgressTips: 1, // 默认为1，显示进度提示
				success: res => {
					
					this.serverId = res.serverId; // 返回音频的服务器端ID
                    console.log('返回了serverID', this.serverId)
					this.uplodAudio()
				},
				fail: err =>{
					_this.touchendClick = true
				}
			})
		},
        collectOrder(title) {
			const consultID = this.id
			this.operaShow = false
            this.instance.collectConsultation({
				consultID
            })
                .then((response) => {
					if (title === '收藏') {
						Toast({
							message: '收藏成功',
							duration: 1000
						})
					} else {
						Toast({
							message: '取消成功',
							duration: 1000
						})
					}
					this.consultationDetail()
            
                })
                .catch((error) => {
                }) 
            
		},
		callPhone () { // 电话咨询拨打电话
			let phoneN = this.caller
			let mes = `您的当前呼叫手机号是否为${phoneN}？`
			let _this = this
			MessageBox.confirm(mes).then(action => {
				_this.palyPhone()
            },function(){
				MessageBox.confirm('去维护新的手机号？').then(action => {
					_this.$router.push({ path: '/doctor-info-list'})
                },function(){
                    console.log('取消了');
                })
            })

		},
		palyPhone() { // 拨打电话调用
			const caller = this.caller
			const callee = this.callee
			const consultID = this.id
            this.instance.yaeherPhone({
                caller,
				callee,
				consultID
            })
                .then((response) => {
                    if(response.data.result.code === 200) {
                        Toast('拨打成功')
                    }
                    
                })
                .catch((error) => {
                }) 
		},
		consultationDetail (flag) { // 获取咨询详情
			const id = this.id
			let _this = this
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
            this.instance.consultationDetailD(
				params
			)
                .then((response) => {
					let recordDetail = response.data.result.item
					this.recordDetail = recordDetail
					if (id === 0 ) {
						this.id = recordDetail.id
					} else {
						this.consultNumber = recordDetail.consultNumber
					}
					this.getReplayText()
					this.replys = recordDetail.replys
					this.evaluationId = recordDetail.evaluationId
					this.callee = recordDetail.phoneNumber
					this.caller = recordDetail.doctorPhoneNumber
					this.consultationfile = recordDetail.consultationfile
					this.doctorId = recordDetail.doctorID
					this.serviceMoneyListID = recordDetail.serviceMoneyListID
					this.recordDetail.iiInessDescription = this.recordDetail.iiInessDescription.replace(/\n/g,"<br/>")
					this.recordDetail.createdOn = moment(this.recordDetail.createdOn).format('YYYY-MM-DD HH:mm:ss')
					window.sessionStorage.setItem('doctorId', this.doctorId)
					window.sessionStorage.setItem('doctorName', recordDetail.doctorName)
					window.sessionStorage.setItem('doctorAvatar', recordDetail.userImage)
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
					this.$previewRefresh()
					// _this.$nextTick(function(){
					// 	const gallery1 = new Viewer(document.getElementById('imgCaseList'), {
					// 		title: false
					// 	});
					// 	for (let i = 0; i < this.replys.length; i++) {
					// 		new Viewer(document.getElementById(i), {
					// 		title: false
					// 	});
					// 	}
					// })

					if (flag) {
						_this.$nextTick(function(){
						})
					}
					
					if (recordDetail.refundReason) {
						this.refundReason = JSON.parse(recordDetail.refundReason).LabelName
					}
                })
                .catch((error) => {
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
		upReplayText (replayTxt) { // 更新回复缓存
			const consultNumber = this.consultNumber
			const repayIllnessDescription = replayTxt
            this.instance.consultReplyMemoryD({
				consultNumber,
				repayIllnessDescription
            })
                .then((response) => {
					if(response.data.result.code === 200) {
						this.replayDemo = this.repayIllnessDescription
					}
                })
                .catch((error) => {
                }) 
		},
		getReplayText () { // 获取回复缓存
			const consultNumber = this.consultNumber
			const repayIllnessDescription = ''
            this.instance.getReplyMemoryD({
				consultNumber,
				repayIllnessDescription
            })
                .then((response) => {
					if(response.data.result.code === 200) {
						this.repayIllnessDescription = response.data.result.item
						this.replayDemo = this.repayIllnessDescription
					}
					
                })
                .catch((error) => {
                }) 
		},
		deteleteReplayText () { // 删除回复缓存
			const consultNumber = this.consultNumber
            this.instance.removeConsultReplyMemoryD({
				consultNumber
            })
                .then((response) => {
					this.repayIllnessDescription = ''
					this.replayDemo = ''
                })
                .catch((error) => {
                }) 
		},
		searchKeypress (event) {
            if (event.keyCode == 13) {
            } 
        },
		consultationReply (flag) { // 提交回答
			let _this = this
			_this.textShow = false
			let imgList = this.imgList
			let attach = new Array()
            for (var i = 0; i < this.imgList.length; i ++) {
                let obj = {
					id: 0,
                    filename: this.imgList[i].filename,
                    mediaType: this.imgList[i].mediaType,
                    fileSize: this.imgList[i].fileSize
                }
                attach.push(obj)
            }
			const replyStateCode = this.replyState[1].code
			const replyTypeCode = this.replyType[0].code
			let repayIllnessDescription = this.repayIllnessDescription
			if(flag){
				repayIllnessDescription = ''
			} else {
				repayIllnessDescription = this.repayIllnessDescription
			}
			const consultID = this.id
			if(!repayIllnessDescription && attach.length === 0) {
				Toast('回复内容不能为空')
				return
			}
			// let fontFlag = fontVery(repayIllnessDescription)
			// console.log(fontFlag)
            // if (!fontFlag) return
			if(!this.clickTag) {
                MessageBox('提示信息', '请不要重复提交')
                return
			}
			// MessageBox.confirm(repayIllnessDescription).then(action => {
			if (!flag) {
				_this.$indicator.open({
					text: '提交中请稍候',
					spinnerType: 'fading-circle'
				})
			}
			this.clickTag = false
			this.instance.createConsultationReplyD({
				consultID,
				replyStateCode,
				replyTypeCode,
				repayIllnessDescription,
				attach
			})
				.then((response) => {
					
					_this.$indicator.close()
					if(flag){
						
					} else {
						_this.repayIllnessDescription = ''
						_this.deteleteReplayText()
						_this.$refs.address.blur()
					}
					Toast('提交成功')
					
					_this.clickTag = true
					setTimeout(function() {
						// location.reload()
						_this.imgList = []
						_this.consultationDetail(true)
						
					}, 500)
					
				})
				.catch((error) => {
					Toast('提交失败')
					_this.textShow = false
					_this.$indicator.close()
					_this.imgList = []
					_this.clickTag = true
				}) 
            // },function(){
			// 	_this.$indicator.close()
			// 	_this.imgList = []
            //     console.log('取消了')
            // })
			
		},
		uplodAudio() { // 提交语音回复
			let _this = this
			_this.$indicator.open({
				text: '提交中请稍候',
				spinnerType: 'fading-circle'
			})
			const replyStateCode = this.replyState[1].code
			const replyTypeCode = this.replyType[0].code
			const repayIllnessDescription = ''
			const consultID = this.id
			let attach = [
				{
					id: 0,
					filename: this.serverId,
					mediaType: 'voice'
				}
			]
			this.instance.createConsultationReplyD({
				consultID,
				replyStateCode,
				replyTypeCode,
				repayIllnessDescription,
				attach
            })
                .then((response) => {
					_this.touchendClick = true
					_this.$indicator.close()
					Toast('提交成功')
					setTimeout(function() {
						_this.consultationDetail()
					}, 500)
					
                })
                .catch((error) => {
					_this.touchendClick = true
					_this.$indicator.close()
                }) 

		},
		uploadImgOss(evt, index2) { // 选择图片
			let _this = this
			this.$indicator.open({
				text: '提交中请稍候',
				spinnerType: 'fading-circle'
			})
			this.showBtn = false
            let file = evt.target
			const fileList = file.files
			let sumImg = 0
			if (fileList.length > 9) {
				sumImg = 9
				Toast('图片一次最多上传9张')
			} else {
				sumImg = fileList.length
			}
            for(let i = 0; i < sumImg; i++) { // 添加多个图片
                let imgType = /^image\//
                let videoType = /^video\//
                let file1 = file.files[i]
                let fileName = fileList[i].name
                let suffix = fileName.split('.')
                suffix = suffix.pop()

                if (suffix === 'webp') {
                    Toast('暂不支持webp格式的图片')
                    return
                }
                let obj = timestamp()  
				let filename = `wx-${obj}.${suffix}`
				let fileSize = file.files[i].size
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
				_this.clickFlag = false
                let reader = new FileReader()
                reader.readAsDataURL(file1)
                reader.onload = function () {
                    let obj = {
                        fileSize,
                        file1,
                        filename,
                        storeAs,
                        mediaType,
						src: this.result,
						upFlag: false
					}
					if (mediaType === 'image') {
						let result = this.result;
						let img = new Image();
						img.src = result;
						img.onload = function () {
							if (!Orientation || Orientation === 1) {
							} else {
								obj.src = _this.compress(img, Orientation);
								obj.file1 = dataURLtoBlob(obj.src)
							}
							_this.imgList.push(obj)
							console.log(_this.imgList[i], i)
							setTimeout(function(){
								_this.uploadCos(_this.imgList[i].storeAs, _this.imgList[i].file1, _this.imgList[i].mediaType, i)
							}, 200)
							
						}
					}
					
                }
			}
        },
        uploadCos (key, body, type, index) { // 上传图片
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
                    let progress = progressData.percent*100
					console.log(progress)
                }
            }, function(err, data) {
                if (err) {
					_this.clickFlag = true
					Toast('图片发送失败')
					_this.$indicator.close()
					_this.$refs.fileCase.value =''
					console.log(err)
                } else {
					console.log(data)
					_this.imgList[index].upFlag = true
                    let flagAll = true
                    for (let i = 0; i < _this.imgList.length; i++) {
                        flagAll = flagAll && _this.imgList[i].upFlag
                        console.log(i, flagAll)
                    }
					
                    if (flagAll && !_this.clickFlag) {
                        _this.clickFlag = true
                        console.log('文件成功上传完毕')
						_this.consultationReply(true)
                    }
                }
            })
        },
        deleteImg (index) { // 删除图片
            this.imgList.splice(index, 1)
        },
        getTypeParams () { // 获取type参数
            this.instance.tencentCosAccessTokenType({
            })
                .then((response) => {
					let paramsObject = response.data.result.item
                    this.mediaType = paramsObject.mediaType
					this.serviceType = paramsObject.type[2].code
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
            this.instance.tencentCosAccessToken({
                mediaType,
                serviceType
            })
                .then((response) => {
                   const uploadParams = response.data.result.item
				   this.bucket = uploadParams.bucket
				   if (index === 0) {
                       this.fileFolder = uploadParams.fileFolder
                   } else {
                       this.fileFolder1 = uploadParams.fileFolder
                   }
                   this.region = uploadParams.region
                   this.secretId = uploadParams.secretId
                   this.secretKey = uploadParams.secretKey
                })
                .catch((error) => {
                }) 
		},
		getHeight (el) {
			this.timer = setTimeout(() => {
				el.style.height = 'auto' // 必须设置为auto
				if(el.scrollHeight <= 128) {
					el.style.height = (el.scrollHeight) + 'px'
				} else {
					el.style.height = 128 + 'px'
				}
			}, 20)
		},
		getHeight1 (el, el1) {
			this.timer1 = setTimeout(() => {
				el.style.height = 'auto' // 必须设置为auto
				if(el1.scrollHeight <= 128) {
					el.style.height = (el1.scrollHeight) + 'px'
				} else {
					el.style.height = 128 + 'px'
				}
				
			}, 20)
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
        }
	},
	watch: {
		'repayIllnessDescription': function (newVal) {
			this.showBtn = false
			this.getHeight(this.$refs.address)
			this.getHeight1(this.$refs.sendOut, this.$refs.address)
			this.getHeight1(this.$refs.heightAdd, this.$refs.address)
			this.getHeight1(this.$refs.inputChat, this.$refs.address)
		},
        $route(now, old) {
			console.log(now.name)
			if (now.name === 'orderDetail') {
				document.body.style.overflow = 'auto'
			} else {
				document.body.style.overflow = 'hidden'
			}
        }
	},
	mounted () {
		this.consultationDetail()
		this.replyParameter()
		this.getTypeParams()
		let _this = this
		const url = this.urlWx
		interval = window.setInterval(function() {
			if (_this.replayDemo != _this.repayIllnessDescription) {
				_this.upReplayText(_this.repayIllnessDescription)
			}
		}, 10000)
        this.instance.wXJSTicket({
            url
        })
            .then((response) => {
                if(response.data.result.code === 200) {
                    let WxSign = response.data.result.item
                    let appId = WxSign.appId
                    let timestamp = WxSign.timestamp
                    let nonceStr = WxSign.nonceStr
                    let signature = WxSign.signature
                    let params = {
                        appId,
                        timestamp,
                        nonceStr,
                        signature
                    }
                    that.config({
                        debug: false,
                        appId,
                        timestamp,
                        nonceStr,
                        signature,
                        jsApiList: ['startRecord', 'stopRecord', 'onVoiceRecordEnd', 'playVoice', 'uploadVoice']
                    })
                    that.ready(() => {
						let localFlag = window.localStorage.getItem('localYes')
						if (localFlag != 'yes'){ // 判断是否录音过，录音过不再授权录音
							that.startRecord()
							setTimeout(function() {
								that.stopRecord({ // 微信结束录音接口
									success: res => {
										this.localId = ''
										let localYes = 'yes'
										window.localStorage.setItem('localYes', localYes)
									}
								})
								console.log('自动调用录音结束')
							}, 300)
						}
                    })

                    that.error((res) => {
                        // alert('出错了：' + res.errMsg)
                    })
                }
                
            })
            .catch((error) => {
            }) 
	},
	beforeDestroy() {
		clearInterval(interval);
	},
	created () {
		this.urlWx = window.location.href.split('#')[0]
		let id = this.$route.query.id
		if (id) {
			this.id = parseInt(id)
		} else {
			// this.consultNumber = 'CN-201812171004JTAI'
			this.consultNumber = window.sessionStorage.getItem('consultNumber')
			this.returnShow = false
		}
	}
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.orderDetail {
	// padding: 42px 0 0;
	//overflow: hidden;
	// height: 100%;
	
	// position: relative;
	// -webkit-appearance: none;
	position: absolute;
	top: 0;
	left: 0;
	right: 0;
	bottom: 0;
	background: #fff;
	z-index: 2;
	.textShow {
		position: fixed;
		top: 0;
		left: 0;
		right: 0;
		bottom: 0;
		background: #fff;
		z-index: 2000;
		
		.introduce-case {
			padding: 10px 10px 20px; 
			background: $color-wfont;
			.mint-cell-wrapper {
				background-image: none;
			}
			.mint-cell-title {
				display: none;
			}
			.okCase {
				padding-top: 20px;
			}
			p {
				text-align: right;
				color: $color-bfont;
				font-size: $font-m;
				padding-right: 5px;
			}
			// .mint-field-core {
			// 	overflow: auto;
			// 	-webkit-overflow-scrolling: touch;
			// }
		}
		.sendPreview {
			text-align: center;
			font-size: $font-xxl;
			padding: 1px 0 8px;
		}
		.textContent {
			font-size: $font-t;
			line-height: 1.4;
			position: absolute;
			top: 0;
			left: 0;
			right: 0;
			bottom: 50px;
			padding: 20px 20px 10px;
			overflow: auto;
			-webkit-overflow-scrolling: touch;
			.mint-cell-title {
				display: none;
			}
			// textarea {
			// 	border: none; 
			// 	font-size: 18px; 
			// 	outline: none; 
			// 	padding: 0px;
			// 	resize: none; 
			// 	width: 100%; 
			// 	box-sizing: border-box;
			// 	height: 100%;
			// }
		}
	}
	.contCase {
		position: absolute;
		left: 0;
		right: 0;
		bottom: 45px;
		top: 42px;
		padding-bottom: 0;
		overflow: auto;
		-webkit-overflow-scrolling: touch;
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
	.moreHeight {

		height: 400px;
	}
	.callTimeList{font-size: $font-l;}
	.condition {margin: 10px 0 0; background: $color-wfont; font-size: $font-t; color: #929ba2;}
	.conditionP {padding-bottom: 10px;}
	.condition span {color: #000;}
	.condition .applyTime {float: right; }
	.conditionP1 span {font-size: $font-m; background: $default-color; color: #fff; float: right; display: inline-block; width: 60px; height: 20px;text-align: center;line-height: 21px;border-radius: 3px;}
	.conditionP1 .callHint{font-style: normal; font-size: $font-m; }
	.condition .paList li {border-top: 1px dashed #eee; float: left; padding: 10px 0; border-bottom: none; width: 100%;}
	.paList .illCase {
		line-height: 20px;
		padding: 0 0px; 
		display: block;
		color: #333;
	}
	.selectYes {
		-webkit-user-select: text;
		-moz-user-select: text;
		-ms-user-select: text;
		user-select: text;
	}
	.condition .paList .shortLi {width: 50%;}
	.condition .paList .border-none {border: none;}
	img {border-radius: 3px;}
	.zhuiWen { padding: 10px 15px; background: #fff;}
	.zhuiWen div {padding: 10px 0; font-size: 18px; color: #000;}
	.zhuiWen div span{float: right; font-size: $font-l;}
	.zhuiWen .zhuiContent {color: $color-afont; font-size: 17px; color: #151515; padding: 5px 0 0 0;}
	// .zhuiWen img { width: 100px;}
	// .replaysBox {
	// 	// overflow: hidden;
	// 	padding: 0 0 0 5px;
	// 	background: $default-color;
	// 	li {
	// 		float: left;
	// 		width: 80px;
	// 		height: 80px;
	// 		padding: 5px 5px 0px 0;
	// 		img{
	// 			// width: 100%;
	// 			height: 100%;
	// 			border-radius: 5px;
	// 		}
	// 		video {
	// 			width: 100%;
	// 			height: 100%;
	// 		}
	// 	}
	// 	.voiceLi {
	// 		width: auto;
	// 		height: 20px;
	// 		padding: 0;
	// 		position: relative;
	// 		.timeVoice {
	// 			position: absolute;
	// 			top: 0;
	// 			left: -40px;
	// 			color: $color-bfont;
	// 			text-align: right;
	// 		}
	// 	}
	// }
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
        width: 18px;
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
			// padding: 5px 9px 5px 0;
		}
	}
	.paList {padding: 0 10px; overflow: hidden;}
	.paList .imgCaseList {padding: 5px 0; overflow: hidden;}
	.paList .imgCaseList li {border-top: none;}
	.paList .imgCaseList .showImg {float: left; width: 100px; height: 100px; margin: 5px 9px 0px 0; padding: 0; overflow: hidden;}
	.paList .imgCaseList .showImg:nth-child(3n) {padding-right: 0px;}
	.paList .imgCaseList .showImg {
		position: relative;
		border-radius: 3px;
		img{
			// width: 100%;
            position: absolute;
            top: 50%;
            left: 50%;
            display: block;
			height: 100%;
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
	.weChat {padding: 15px; background: #fff; overflow: hidden;}
	.weTime {width: 100%; float: right; display: flex; justify-content: center; align-items: center; padding-bottom: 7px;}
	.weTime div {padding: 3px 5px; color: #ccc; font-size: $font-m; border-radius: 3px;}
	.weChat p {font-size: $font-t; color: #333;}
	.weTitle {background: #eee;display: inline-block; float: right; padding: 10px; margin-right: 50px; position: relative; border-radius: 5px;}
	// .weTitle img {width: 80px;}
	.weTitle .avatar {position: absolute; right: -50px; top: 0; width: 40px; height: 40px; border-radius: 5px; padding: 0;}
	.weTitle p {width: 100%;word-break: break-all;}
	.heightAdd{width: 100%; height: auto;}
	.sendOut {height: 18px; border-top: 1px solid #ddd; background-color: #fff; position: fixed; text-align: center; padding: 15px 0 13px; bottom: 0%; left: 0%; right: 0%;  z-index: 1000;}
	.sendOut > div { font-size: 13px; line-height: 30px; padding: 0 11px;}
	.sendOut .sendChat {background-color: $color-green; height: 28px; padding: 0 8px; color: #fff;border-radius: 3px; position: absolute; right: 10px; bottom: 8px; z-index: 2000;}
	.sendOut .voiceChat { height: 30px; color: #fff;border-radius: 3px;  position: absolute; left: 10px; bottom: 8px; z-index: 2000;}
	.sendOut .replay01{background: url(../../assets/image/replay01.png) 0 5px no-repeat;background-size: 22px;}
	.sendOut .replay02{background: url(../../assets/image/replay02.png) 0 5px no-repeat;background-size: 22px;}
	.sendOut .replay03{background: url(../../assets/image/replay03.png) 0 5px no-repeat;background-size: 22px;}
	.sendOut .replay04{background: url(../../assets/image/replay04.png) 0 5px no-repeat;background-size: 22px;}
	.sentImg {position: absolute; height: 30px; right: 60px; padding: 0 8px; bottom: 8px; z-index: 2000;}
	.sendOut .inputChat { height: 18px; font-size: 16px; border-bottom: 1px solid #bbb; background: #fff; border-radius: 0px; padding: 4.5px 0px 6.5px; line-height: 17px; position: absolute; left: 35px; right: 85px; bottom: 8px; z-index: 2;}
	.inputChat textarea {border: none; font-size: 18px; outline: none; padding: 0 5px;resize: none; width: 100%; box-sizing: border-box; display: inline; position: absolute; bottom: 5px; left: 0px; right: 0px; }
	.sendOut .inputChat2 { height: 18px; font-size: 16px; border-bottom: 1px solid #bbb; background: #fff; border-radius: 0px; padding: 4.5px 0px 6.5px; line-height: 17px; position: absolute; left: 35px; right: 85px; bottom: 8px; z-index: 2;border: 1px solid #bbb; left: 45px; right: 95px; border-radius: 3px; line-height: 19px;}
	.operation-case {position: fixed; font-size: $font-l; top: 42px; right: 0px; z-index: 10;  color: $color-wfont;}
	.operation-case ul {background: rgba(0,0,0,0.5); border-radius: 3px;}
	.operation-case li {padding: 10px; text-align: center;}
	.illness-issue {margin: 20px 10px;}
	.returnVisit {background-color: #fff; padding: 10px; font-size: $font-l;}

	.sendOut .upload-row {position: absolute; top: -53px; left: 0; right: 0; background: $color-wfont; padding: 10px 10px;}
    .uploadImgList { padding: 10px 5px; display: flex; flex-wrap:wrap;}
    .uploadImgList li {padding: 5px 5px; border-bottom: none;}
    .uploadImgList li img {display: block; width: 100px; height: 80px;}
    // .uploadImg {padding-bottom: 15px; overflow: hidden;}
    // .uploadImg > label {float: left; margin-right: 10px; position: relative;}
    // .uploadImg input{visibility: hidden; position: absolute; display: block; width: 100%; height: 100%;}
    .uploadImgCase {width: 50px; height: 50px; background: url(../../assets/image/add-pic.png) no-repeat;background-size: 100%;}
    .uploadVideoCase {width: 50px; height: 50px; background: url(../../assets/image/add-pic.png) no-repeat;background-size: 100%;}
	.imgListCase {
        position: relative;
        .deleteBtn {
            position: absolute;
            right: -2px;
            top: 2px;
            background-color: #ff0000;
            display: inline-block;
            color: #fff;
            width: 20px;
            height: 20px;
            border-radius: 10px;
            text-align: center;
            line-height: 21px;
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

	.uploadRow {
		text-align: right;
	}
	.uploadBox {
        width: 90px;
        height: 32px;
        background: $default-color;
        position: relative;
        display: inline-block;
        line-height: 34px;
        text-align: center;
        color: #fff;
        border-radius: 5px;
		margin-right: 5px;
        input{
            visibility: hidden;
            position: absolute; display: block; width: 100%; height: 100%;
        }
	}
	.focusAbsoult {
		position: absolute;
	}
}

</style>