<template>
    <div class="upload-case">
        <!-- <div class="content">
            <div v-for="(item, index) in audioList" :key="index" class="videoBtn">
                <span @click="playAudio(index ,item.src, item.play)" class="voiceSpan" :class="{voicePlay: item.play}"></span>
                
			</div>
            <div>
                <audio ref="audioCase" id="audioCase" src=""></audio>
            </div>
            <div class="videoBtn">
				<mt-button @click="gtouchstart" type="primary" class="mint-button--large">开始录音</mt-button>
			</div>
            <div class="videoBtn">
				<mt-button @click="gtouchend" type="primary" class="mint-button--large">停止录音</mt-button>
			</div>
            <div class="videoBtn">
				<mt-button @click="playVioce1" type="primary" class="mint-button--large">播放录音</mt-button>
			</div>
            <div class="videoBtn">
				<mt-button @click="wxUpload" type="primary" class="mint-button--large">上传录音</mt-button>
			</div>
            <div class="videoBtn">
                <div class="btnVicos" @touchstart="gtouchstart" @touchend="gtouchend" @touchmove="gtouchmove">录音</div>
            </div>
            <div class="videoBtn">
                时长: {{time}}
			</div>
            <div class="videoBtn">
                mesText: {{mesText}}
			</div>
            <div class="videoBtn">
                localId: {{this.localId}}
			</div>
            <div class="videoBtn unselect">
                serverId: {{this.serverId}}
			</div> -->
            <!-- <div v-if="show" class="cropperBox">
                <vueCropper
                    ref="cropper"
                    :img="example2.img"
                    :outputSize="example2.size"
                    :outputType="example2.outputType"
                    :info="example2.info"
                    :canScale="example2.canScale"
                    :autoCrop="example2.autoCrop"
                    :autoCropWidth="example2.autoCropWidth"
                    :autoCropHeight="example2.autoCropHeight"
                    :fixed="example2.fixed"
                    :fixedNumber="example2.fixedNumber"
                ></vueCropper>
            </div>
            <div v-if="show"  class="buttomCase">
                <button @click="caijian">裁剪</button>
            </div>
            <div class="">
                <button @click="caijian1">打开裁剪</button>
                <div>
                    <audio controls src="/static/audios/test.mp3"></audio>
                </div>
                <img width="100px;" src="../../assets/image/article1.jpg" />
            </div>
            <div class="imgCase">
                <img :src="srcImg" />
            </div> -->
            
        <!--                 
            <img v-gallery style="width: 80px; padding: 0 10px;"  :src="srcImg" />
                <div><video width="350" height="200" :src="srcVideo" controls></video></div>
                <div>
                    <video width="350" height="200" :src="srcVideo1" controls >
                    </video>
                </div>
                <mt-progress :value="progress">
                    <div slot="start">0%</div>
                    <div slot="end">100%</div>
                </mt-progress>
            <div><input style="margin: 10px 0;" accept="image/*" type="file" v-on:change="uploadImgOss($event, 1)"/>上传图片</div>    
            
            <div><input accept="video/*" type="file" v-on:change="uploadImgOss($event, 2)"/> 上传视频</div> -->
      <!-- </div> -->
  </div>
</template>

<script>
import wx from 'weixin-js-sdk'
import { VueCropper }  from 'vue-cropper' 
import { Toast } from 'mint-ui';
let that = wx
let timer 
export default {
    components: { 
      VueCropper 
    }, 
    data () {
        return {
            videoSrc: '',
            // audioList: [
            //     {
            //         id: 1,
            //         src: 'http://sc1.111ttt.cn:8282/2018/1/03m/13/396131229550.m4a?tflag=1519095601&pin=6cd414115fdb9a950d827487b16b5f97#.mp3',
            //         play: false
            //     },
            //     {
            //         id: 2,
            //         src: 'http://sc1.111ttt.cn:8282/2018/1/03m/13/396131227447.m4a?tflag=1519095601&pin=6cd414115fdb9a950d827487b16b5f97#.mp3',
            //         play: false
            //     },
            //     {
            //         id: 3,
            //         src: 'http://sc1.111ttt.cn:8282/2018/1/03m/13/396131202421.m4a?tflag=1519095601&pin=6cd414115fdb9a950d827487b16b5f97#.mp3',
            //         play: false
            //     }
            // ],
            // srcImg: '',
            // show: false,
            // example2: {
            //     img: '../../assets/image/article1.jpg',
            //     info: true,
            //     size: 1,
            //     outputType: 'jpeg',
            //     canScale: false,
            //     autoCrop: true,
            //     // 只有自动截图开启 宽度高度才生效
            //     autoCropWidth: 300,
            //     autoCropHeight: 250,
            //     // 开启宽度和高度比例
            //     fixed: true,
            //     fixedNumber: [2, 2]
            // }
            // srcImg: '',
            // poster = "../../assets/image/article1.jpg"
            // srcVideo: 'http://video.pearvideo.com/mp4/third/20181113/cont-1475423-10477784-170431-hd.mp4',
            // srcVideo: 'http://video.pearvideo.com/mp4/adshort/20181111/cont-1473641-11859924-155605_adpkg-ad_hd.mp4',
            // index: 0,
            // imgList: [],
            // srcVideo1: '',
            // fileType: 0,
            // progress: 0,
            // mediaType: '',
            // serviceType: ''
            urlWx: '',
            localId: '',
            serverId: '',
            wxConfig: {},
            mesText: '',
            breakOff: true,
            time: 0,
            audioPlay: false
        }
    },
    methods: {
        // getConfig() { // 获取jssdk参数
		// 	const url = this.urlWx
        //     this.instance.wXJSTicket({
        //         url
        //     })
        //         .then((response) => {
        //             if(response.data.result.code === 200) {
		// 				this.wxConfig = response.data.result.item
		// 				console.log(this.wxConfig)
		// 				this.voiceClick()
        //             }
                    
        //         })
        //         .catch((error) => {
        //         }) 
		// },
		// voiceClick () {
		// 	let WxSign = wxConfig
		// 	let appId = WxSign.appId
		// 	let timestamp = WxSign.timestamp
		// 	let nonceStr = WxSign.nonceStr
		// 	let signature = WxSign.signature
		// 	that.config({
		// 		debug: false,
		// 		appId,
		// 		timestamp,
		// 		nonceStr,
		// 		signature,
		// 		jsApiList: ['startRecord', 'stopRecord', 'onVoiceRecordEnd', 'playVoice', 'uploadVoice']
		// 	})
		// 	that.ready(function () {

        //     })
        //     that.error(function (res) {
        //         alert('出错了：' + res.errMsg)
        //     })
        // },
        playAudio (index, src, playFlag){
            // let audioCase = this.$refs.audioCase
            let _this= this
            let audioCase1 = document.getElementById('audioCase')
            
            // console.log(audioCase)
            // console.log(audioCase1)
            this.$refs.audioCase.src = src
            
            setTimeout(function() {
                // audioCase1.play()
                // audioCase1.pause()
                // audioCase1.play()
                if (playFlag) {
                    _this.audioList[index].play = false
                    audioCase1.pause()
                } else {
                    audioCase1.play()
                    for(let i = 0; i < _this.audioList.length; i++) {
                        if (i === index) {
                            _this.audioList[index].play = true
                            console.log(_this.audioList)
                        } else {
                            _this.audioList[i].play = false
                        }
                    }
                }
            }, 100)

            
            // console.log(this.audioList)
            // if (!this.currentAudio) {
            //     this.currentAudio = new Audio(src)
            // }
            // // this.currentAudio = src
            // if (playFlag) {
            //     this.audioList[index] = false
            // } else {
            //     for(let i = 0; i < this.audioList.length; i++) {
            //         if (i === index) {
            //             this.audioList[index].play = true
            //             this.currentAudio.play()
            //         } else {
            //             this.audioList[i].play = false
            //             this.currentAudio.pause()
            //         }
            //     }
            // }

            // if (!this.audioPlay) {
            //     this.audioPlay = true
            //     this.currentAudio.play()
            // } else {
            //     this.audioPlay = false
            //     this.currentAudio.pause()
            // }
        },
        gtouchstart(){
            let _this = this
            this.time = 0
            // this.$indicator.open({
            //     text: '正在录音,滑动取消',
            //     spinnerType: 'double-bounce'
            // })
            this.mesText = '点了录音'
            console.log('点了录音')
            Toast('开始录音')
            // timer = window.setInterval(function() {
            //     if ((_this.time++) >= 30) {
            //         _this.gtouchend()
            //         window.clearInterval(timer)
            //     }
            // }, 1000)
            that.startRecord()
			// 当用户长按500ms以上再真正开始录音
			// setTimeout(() => {
			// 	this.longPress()
            // }, 500)
            
            
		},
		longPress(){
			that.startRecord() // 微信开始录音接口
		},
		gtouchmove(){
            this.mesText = '中断录音'
            this.$indicator.close()
            this.breakOff = false
            window.clearInterval(timer)
            Toast('录音已取消')
			that.stopRecord({ // 微信结束录音接口
				success: res => {
					this.localId = '';
					console.log('中断结束录音localId',this.localId)
					console.log('res', res)
				}
			})
		},
		gtouchend(){
            Toast('结束录音')
            // window.clearInterval(timer)
            if(this.breakOff) {
                this.mesText = '结束录音'
                // this.$indicator.close()
                that.stopRecord({ // 微信结束录音接口
                    success: res => {
                        this.localId = res.localId;
                        console.log('正常结束录音成功了',this.localId)
                        // console.log('res', res)
                        // alert(this.localId)
                    }
                })
            }
            
        },
        playVioce1() { // 播放录音
            // Toast('播放录音')
            this.mesText = '播放录音'
            that.playVoice({
                localId: this.localId
            });
        },
        wxUpload() { // 上传到微信服务器
            // Toast('上传录音')
            this.mesText = '上传录音'
			that.uploadVoice({
				localId: this.localId, // 需要上传的音频的本地ID，由stopRecord接口获得
				isShowProgressTips: 1, // 默认为1，显示进度提示
				success: res => {
					this.serverId = res.serverId; // 返回音频的服务器端ID
                    console.log('返回了serverID', this.serverId)
                    // alert(this.serverId)
				}
			})
		},
        // caijian1 () {
        //     this.example2.fixedNumber = [375, 230]
        //     this.show = true
        // },
        // caijian () {
        //     this.$refs.cropper.getCropData((data) => {
        //         this.srcImg = data
        //     console.log(data)  
        //     })

        //     this.$refs.cropper.getCropBlob((data) => {
        //     // do something
        //     console.log(data)  
        //     })
        //     this.show = false
        // }
    
        // getTypeParams () { // 获取type参数
        //     this.instance.tencentCosAccessTokenType({
        //     })
        //         .then((response) => {
        //             this.mediaType = response.data.result.item.mediaType[0].code
        //             this.serviceType = response.data.result.item.type[0].code
        //             this.getUploadParams()
        //             console.log(this.mediaType)
        //             console.log(this.serviceType)
        //         })
        //         .catch((error) => {
        //         }) 
        // },
        // getUploadParams () { // 获取上传参数
        //     const serviceType = this.serviceType
        //     const mediaType = this.mediaType
        //     const secret = createSecret()
        //     this.instance.tencentCosAccessToken({
        //         secret,
        //         mediaType,
        //         serviceType
        //     })
        //         .then((response) => {
        //            const uploadParams = response.data.result.item
        //            this.bucket = uploadParams.bucket
        //            this.fileFolder = uploadParams.fileFolder
        //            this.region = uploadParams.region
        //            this.secretId = uploadParams.secretId
        //            this.secretKey = uploadParams.secretKey
        //             console.log(uploadParams)
        //         })
        //         .catch((error) => {
        //         }) 
        // },
        // uploadImgOss(evt, index) {
        //     let maxsize = 5*1024*1024
        //     let file = evt.target
        //     let _this = this
        //     let fileName = file.files[0].name
        //     let filesize = file.files[0].size
        //     let file1 = file.files[0]
        //     let imgType = /^image\//
        //     let videoType = /^video\//
        //     console.log(imgType.test(file1.type))
        //     console.log(index)
        //     if (imgType.test(file1.type) && index === 1) {
        //         console.log('上传了图片')
        //     } else if (videoType.test(file1.type) && index === 2) {
        //         console.log('上传了视频')
        //     } else {
        //         console.log('请上传正确文件')
        //         return
        //     }
        //     let suffix = fileName.substr(fileName.indexOf('.'))
        //     let obj = this.timestamp()  
        //     let storeAs = `${this.fileFolder}/wx-${obj}${suffix}`  
        //     let reader = new FileReader()
        //     reader.readAsDataURL(file1)
        //     reader.onload = function () {
        //         if (index === 1) {
        //             _this.srcImg = this.result
        //         } else if (index === 2) {
        //             _this.srcVideo1 = this.result
        //             console.log('video')
        //         }
                
                
        //     }
            
        // },
        // uploadCos (key, body, type) {
        //     // 初始化实例
        //     let _this = this
        //     var COS = require('cos-js-sdk-v5')
        //     var cos = new COS({
        //         SecretId: this.secretId,
        //         SecretKey: this.secretKey,
        //     })
        //     cos.putObject({
        //         Bucket: this.bucket, /* 必须 */
        //         Region: this.region,    /* 必须 */
        //         Key: key,              /* 必须 */
        //         Body: body, // 上传文件对象
        //         onProgress: function(progressData) {
        //             console.log('上传进度', progressData.percent*100)
        //             if (index === 2) {
        //                 _this.progress = progressData.percent*100
        //             }
        //         }
        //     }, function(err, data) {
        //         console.log(err || data)
        //         if (err) {
        //             console.log(err)
        //         } else {
        //             console.log(data)
        //             let reader = new FileReader()
        //             reader.readAsDataURL(body)
        //             reader.onload = function () {
        //                 if(type === 1) {
        //                     _this.srcImg = this.result
        //                 } else {
        //                     _this.srcVideo = this.result
        //                 }
                        
        //             }
        //         }
                
        //     })
            
        // },
        // deleteImg (index) {
        //     this.imgList.splice(index, 1)
        // },
        // timestamp () {
        //     let _this = this
        //     var time = new Date()
        //     var y = time.getFullYear()
        //     var m = time.getMonth() + 1
        //     var d = time.getDate()
        //     var shijianChuo = Date.parse(new Date())
        //     return `${y}${_this.add0(m)}${_this.add0(d)}-${shijianChuo}`
        // },
        // add0 (m) {
        //     return m < 10 ? `0${m}` : m
        // }
    },
    mounted () {
        const url = this.urlWx
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
                    console.log('params', params)
                    that.config({
                        debug: false,
                        appId,
                        timestamp,
                        nonceStr,
                        signature,
                        jsApiList: ['startRecord', 'stopRecord', 'onVoiceRecordEnd', 'playVoice', 'uploadVoice']
                    })
                    that.ready(() => {
                        // that.startRecord()
                    })

                    that.error((res) => {
                        alert('出错了：' + res.errMsg)
                    })
                }
                
            })
            .catch((error) => {
            }) 
    },
    created () {
        // this.urlWx = encodeURIComponent(window.location.href.split('#')[0])
        this.urlWx = window.location.href.split('#')[0]
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";
.upload-case {
    background: #fff;
    -webkit-user-select: text;
    -moz-user-select: text;
    -ms-user-select: text;
    user-select: text;
    .videoBtn {
        padding: 20px 15px;
    }
    // .unselect {
    //     -webkit-user-select: text;
    //     -moz-user-select: text;
    //     -ms-user-select: text;
    //     user-select: text;
    // }
    .btnVicos {
        background: $default-color;
        color: #fff;
        font-size: $font-l;
        text-align: center;
        padding: 10px 0;
    }
    .cropperBox {
        position: fixed;
        left: 0;
        bottom: 0;
        right: 0;
        top: 0;
        z-index: 2;
    }
    .imgCase {
        width: 100px;
        img {
            width: 100%;
        }
    }
    .buttomCase {
        position: fixed;
        bottom: 10px;
        left: 20px;;
        z-index: 3;
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
        background-color: #111;
    }
    .voicePlay{
        /*播放中（不需要过渡动画）*/
        /*background-position: 0px -0px;*/
        -webkit-animation: voiceAnimitation 0.8s infinite step-start;
        -moz-animation: voiceAnimitation 0.8s infinite step-start;
        -o-animation: voiceAnimitation 0.8s infinite step-start;
        animation: voiceAnimitation 0.8s infinite step-start;
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
}

</style>
