<template>
    <div class="order-control padding-top">
		<mt-header fixed title="咨询详情">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
            <mt-button v-if="recordDetail.refundCheckState === '' && admin !='admin'"  slot="right">
                <router-link :to="{path: '/chargeback', query: {id: id, chargeType: 'control'}}" class="right-white">退单</router-link>
            </mt-button> 
        </mt-header>
        <div class="content">
            <ul class="mui-table-view">
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
						<router-link :to="{path: '/doctor-detail-patient', query: {id: recordDetail.doctorID}}">
							<img class="avatar" :src="recordDetail.userImage" />
						</router-link>
						<p>
							医生在 {{item.createdOn}} 进行了电话回复
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
			<div v-if="recordDetail.isReturnVisit" class="returnVisit">
				<p>
					回访：
					<span class="text-right"></span>
				</p>
				<p class="font-describe">
					{{returnVisit}}
				</p>
			</div>
			<div v-if="recordDetail.refundState === 'return'" class="returnVisit">
				<p v-if="refundReason" class="conditionP">
					退单原因：{{refundReason}}
				</p>
				<p v-if="recordDetail.refundRemarks" class="conditionP">
					退单描述：{{recordDetail.refundRemarks}}
				</p>
			</div>	
            <ul class="paList zhiKong">
                <li v-if="item.replyState === 'treated'" v-for="(item, index) in qualityControlManage" :key="index">
                    <p>质控委员名：{{item.doctorName}}</p>
                    <p>评分：<star :star=item.qualityLevel></star></p>
                    <p>{{item.repayIllnessDescription}}</p>
                </li>
                <li v-if="recordDetail.isQuality">
                    <p>质控评分：<star :star=recordDetail.qualityLevel></star></p>
                    <p>{{recordDetail.qualityReason}}</p>
                </li>
            </ul>
			<div v-if="!recordDetail.isQuality && admin !='admin'" class="notice1">
				<div class="illness border-top" @click="selectSex">
                    <div class="illness-case">
                        <p class="illness-label">选择操作</p>
                        <p class="illness-value green-hint">{{typeOperation}}</p>
                    </div>
                </div>
                <div v-show="operationShow===0" class="border-top">
                    <div class="control-star">
                        进行评分 
                        <div class="give-star">
                            <div @click="playStar(index)" v-for="(value,index) in 5" :key="index" :class="{starOk: value <= giveStar, starNo: value > giveStar}"></div>
                        </div>
                        <mt-button @click="clickStar(true)" type="default">确定</mt-button>
                    </div>
                </div>
                <div v-show="operationShow===1 && replyState!='untreated'" class="border-top">
                    <div class="control-star">
                        转给质控委员
                        <mt-button @click="clickStar(false, '确定转给质控委员?', id)" type="default">确定</mt-button>
                    </div>
                </div>
                <div v-show="operationShow===3" class="border-top">
                    <div class="control-star">
                        推送给医生
                        <mt-button @click="clickStar(false, '确定推送给医生?')" type="default">确定</mt-button>
                    </div>
                </div>
			</div>
        </div>
        <mt-picker :slots="slots" ref="picker" :showToolbar="true" v-show="show">
            <div @click="selectSex(true)" class="slots-no">取消</div>
            <div @click="getPickerValue(true)" class="slots-ok">确认</div>
        </mt-picker>
        <div v-show="false">
			<audio ref="audioCase" id="audioCase" src=""></audio>
		</div>
    </div>
</template>

<script>
import { MessageBox, Toast } from 'mint-ui';
import { formatDate } from 'assets/js/common.js'
import Star from 'components/star/star'
let moment = require('moment');
export default {
    components: {
      Star
    },
    data () {
        return {
            id: 0,
            sex: '',
            returnShow: true,
			videoSrc: '',
			videoShow: false,
			replys: [],
			consultationfile: [],
			recordDetail: {},
			refundReason: '',
			replyType: [],
			consultantJSON: {},
            qualityControlManage: [],
            introduction: '',
            show: false,
            typeOperation: '修改评分',
            admin: '',
            operationShow: 0,
            replyState: '',
            giveStar: 5,
            slots: [
                {
                    flex: 1,
                    values: ['修改评分', '转给质控委员'],
                    className: 'slot1',
                    textAlign: 'center'
                }
            ],
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
        getPickerValue() {     
            const selectArry = ['修改评分', '转给质控委员'];    
            this.show = !this.show
            this.typeOperation = this.$refs.picker.getValues()[0]
            this.operationShow = selectArry.indexOf(this.typeOperation)
        },
        selectSex() {
            this.show = !this.show
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
        clickStar(flag, mesg, id) {
            let _this = this
            if (flag) {
                MessageBox.prompt('请输入评价原因').then(({ value, action }) => {
                    _this.qualityReason = value
                    if (!value) {
                        Toast('评价原因不能为空')
                        return
                    }
                    this.starMessage()
                },function(){
                    console.log('取消了');
                })
            } else {
                MessageBox.confirm(mesg).then(action => {
                    this.$router.push({
                        path: '/control-list',
                        query: {
                            id
                        }
                    })
                },function(){
                    console.log('取消了');
                })
            }
            
        },
        starMessage () { // 评分元素
            const id = this.evaluationId
            const qualityReason = this.qualityReason
            const qualityLevel = this.giveStar
            this.instance.updateConsultationEvaluationC({
                id,
                qualityReason,
                qualityLevel
            })
                .then((response) => {
                    if (response.data.result.code === 200) {
                        Toast('评分成功')
                        this.$router.go(-1)
                    }
                    
                })
                .catch((error) => {
                }) 
        },
        consultationDetail1 () { // 获取咨询详情
			const id = this.id
            this.instance.consultationDetailD({
				id
            })
                .then((response) => {
                    let recordDetail = response.data.result.item
					this.recordDetail = recordDetail
					this.replys = recordDetail.replys
					this.evaluationId = recordDetail.evaluationId
					this.returnVisit = recordDetail.returnVisit
                    this.consultationfile = recordDetail.consultationfile
                    this.qualityControlManage = recordDetail.qualityControlManage
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
                    if ( this.qualityControlManage.length>0) {
                        this.replyState = this.qualityControlManage[0].replyState
                    }
                    
                    if (recordDetail.refundReason) {
                        this.refundReason = JSON.parse(recordDetail.refundReason).LabelName
                        console.log(this.refundReason)
					}
                })
                .catch((error) => {
                }) 
		},
    },
    mounted () {
        this.consultationDetail1()
    },
	created () {
        this.id = parseInt(this.$route.query.id)
        this.admin = this.$route.query.admin
	}
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.order-control {
    .give-star .starOk{width: 20px; height: 20px;}
	.give-star .starNo{width: 20px; height: 20px;}
    .give-star .starBan{width: 20px; height: 20px;}
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
            .doctorAn {
                flex: 1; 
                text-align: right; 
                padding-right: 20px; 
                position: relative;
                .mint-cell-allow-right::after {
                    right: 10px;
                }
                .img {
                    width: 20px; 
                    height: 20px; 
                    border-radius: 3px;
                    display: inline-block;
                    vertical-align: middle;
                }
                span {
                    display: inline-block; 
                    vertical-align: middle;
                }
            }
		}
	}
    .condition {
        margin: 10px 0; 
        padding: 10px; 
        background: $color-wfont; 
        font-size:  $font-l; 
        color: $color-cfont;
        .conditionP {
            padding-bottom: 10px;
            span {
                color: $color-afont;
            }
        }
        .paList {
            overflow: hidden;
            padding: 0;
            li {
                padding: 10px 0;
                color: $color-bfont;
                float: left;
                border-top: 1px dashed #eee; 
                padding: 10px 0; 
                border-bottom: none;
                width: 100%;
                .font-describe {
                    line-height: 20px;
                    padding: 0 10px; 
                    color: $color-afont
                }
            }
        }    
    }
    .condition {margin: 10px 0 0; background: $color-wfont; font-size: $font-l; color: #999;}
	.conditionP {padding-bottom: 10px;}
	.condition span {color: $color-afont;}
	.conditionP1 span {font-size: $font-m; background: $default-color; color: #fff; float: right; display: inline-block; width: 60px; height: 20px;text-align: center;line-height: 20px;border-radius: 3px;}
    .condition .paList .border-none {border: none;}
    .paList .illCase {
		line-height: 20px;
		padding: 0 10px; 
		display: block;
	}
	.condition .paList .shortLi {width: 50%;}
    .doctorAnswer {
        padding: 10px 20px 10px 40px; 
        background: $color-wfont; 
        font-size: $font-m; 
        position: relative;
        p {
            font-size:  $font-l; 
            color: $color-afont;
        }
        .doctorTitle {
            display: flex;
            padding-bottom: 5px;
            p {
                flex: 1; 
                color: $color-afont;
            }
        }
        img {
            position: absolute; 
            top: 10px; 
            left: 10px; 
            width: 20px; 
            height: 20px; 
            border-radius: 5px;
        }
    }
    .notice1 {
        background: $color-wfont; 
        margin: 10px 0;
        padding: 10px 10px 10px;
        .illness {
            background-color: $color-wfont; 
            padding: 0 0 0 10px;
            .illness-case {
                display: flex; 
                padding: 10px 0;
                .illness-label {
                    width: 105px;
                }
            }
        }
        .control-star {
            font-size: $font-l;
            padding: 10px;
            .mint-button {
                height: 30px;
                font-size: $font-l;
                margin-left: 50px;
            }
        }
    }
    .zhiKong {
        margin-top: 10px;
        font-size: $font-l;
    }
    img {border-radius: 3px;}
	.zhuiWen { padding: 10px 15px; background: #fff;}
    .zhuiWen div {border-bottom: 1px dashed #eee;padding: 10px 0; font-size: $font-t; color: $color-bfont;}
    .zhuiWen p {padding-top: 3px;}
	.zhuiWen div span{float: right;}
	.zhuiWen .zhuiContent {color: $color-afont;}
	.zhuiWen img { width: 100px; margin: 5px 5px 0 0;}
	.imgCaseList {padding: 10px;}
	.imgCaseList img {width: 100px; padding-right: 10px}
	.weChat {padding: 10px; background: #fff; overflow: hidden;}
	.weTime {width: 100%; float: right; display: flex; justify-content: center; align-items: center; padding-bottom: 10px;}
	.weTime div {padding: 3px 5px; background: #aaa; color: $color-wfont; font-size: $font-l; border-radius: 3px;}
	.weChat p {font-size: $font-l; color: #333;}
	.weTitle {background: $default-color;display: inline-block; float: right; padding: 10px; margin-right: 50px; position: relative; border-radius: 5px;}
	.weTitle img {width: 80px;}
	.weTitle .avatar {position: absolute; right: -50px; top: 0; width: 40px; height: 40px; border-radius: 5px; padding: 0;}
	.weTitle p {width: 100%;word-break: break-all;}
	.upload-row {position: relative; background: $color-wfont; padding: 10px 10px;}
    .uploadImgList { padding: 10px 5px; display: flex; flex-wrap:wrap;}
    .uploadImgList li {padding: 5px 5px; border-bottom: none;}
    .uploadImgList li img {display: block; width: 100px; height: 80px;}
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
	.condition {margin: 10px 0; padding: 10px; background: $color-wfont; font-size:  $font-l; color: #999;}
	.conditionP {padding-bottom: 10px;}
	.border-bottom {border-bottom: 1px solid #eee;}
	.condition span {color: $color-afont;}
	.condition .paList li {border-bottom: none; border-top: 1px dashed #eee; padding: 10px 0;}
	.font-describe {line-height: 20px;padding: 0 10px; color: $color-afont}
	.mint-cell-title {display: none;}
	.doctorAnswer {padding: 10px 20px 10px 40px; background: $color-wfont; font-size: 12px; position: relative;}
	.doctorAnswer p {font-size:  $font-l; color: $color-afont;}
	.doctorTitle {display: flex; padding-bottom: 5px}
    .doctorTitle p {flex: 1; color: $color-afont;}
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
	.doctorAnswer img {position: absolute; top: 10px; left: 10px; width: 20px; height: 20px; border-radius: 5px;}
	.notice {background: $color-wfont; margin-top: 10px; padding: 10px;}
	.notice .href{width: 100%; height: 40px; color: $color-wfont; text-align: center; line-height: 40px; font-size: $font-xl; background: #35A3FF; display: block; border-radius: 5px;}
	.notice .mui-input-row {height: 160px; padding-bottom: 20px; position: relative;}
	.notice .mui-input-row .hint-font {position: absolute; bottom: 10px; right: 20px; color: #666;}
	.btn-case {padding: 0 10px; text-align: right}
	.notice .btn {display: inline; font-size: $font-l; padding: 5px 8px; border: 1px solid #ccc; line-height: 28px; margin-left: 10px; border-radius: 3px;}
	.agin-evaluate {padding: 10px 0; font-size: $font-l;}
	.agin-evaluate a {color: $default-color;}
	.href {margin-top: 10px;}
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
    .picker {
        position: fixed; 
        bottom: 0; 
        left: 0; 
        right: 0; 
        background: $color-wfont; 
        z-index: 10;
    }
}




</style>