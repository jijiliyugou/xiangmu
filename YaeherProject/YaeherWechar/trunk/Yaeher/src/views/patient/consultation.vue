<template>
    <div class="consultation padding-top">
        <mt-header fixed :title="title">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
            <div class="answer">
                <div class="answer-name">
                    向Ta咨询
                </div>
                <div class="doctorAn">
                    <img :src="doctorInfo.userImage" style="width: 20px; height: 20px; border-radius: 3px;" />
                    <span>{{doctorInfo.doctorName}}</span>
                </div>
            </div>
            <div class="patient-info">
                <div class="illness border-bottom" @click="selectIllness(2)">
                    <div class="illness-case">
                        <p class="illness-label">咨询人</p>
                        <p v-if="!patientName" class="illness-value plValue">请选择咨询人</p>
                        <p v-if="patientName" class="illness-value">{{patientName}}</p>
                    </div>
                </div>
                <div class="illness" @click="selectIllness(1)">
                    <div class="illness-case">
                        <p class="illness-label">疾病类型</p>
                        <p v-if="!iIInessType" class="illness-value plValue">请选择疾病类型</p>
                        <p v-if="iIInessType" class="illness-value">{{iIInessType}}</p>
                    </div>
                </div>
                <mt-field v-if="serviceType1 === 'Phone'" label="手机号" placeholder="请输入手机号" type="tel" v-model="phoneNumber"></mt-field>
                <mt-field label="所在地" placeholder="请输入您所在的城市" v-model="patientCity"></mt-field>
                <mt-field label="问题描述" @input = "descInput" placeholder="请描述症状详情，如出现时间、变化情况，如已就诊请提供医生诊断、病历及当前用药情况，同时请写明需要医生解答的问题。" type="textarea" rows="6" v-model="iIInessDescription" :attr="{ maxlength: maxReplyLength }"></mt-field>
                <div class="wordsNumb">可输入{{remnant}}字</div>
            </div>
            
            <div class="upload-row">
                <label class="uploadBox">
                            图片上传
                    <input ref="imgFile" accept="image/*" type="file" v-on:change="uploadImgOss($event, 'image')"/>
                </label>   
                <div class="uploadRow">
                    <ul class="uploadImgList">
                        <li class="imgListCase"  v-if="orderId===0&&item.mediaType === 'image'&& !item.isDelete"  v-for="(item, index) in imgList" :key="index">
                            <span v-if="item.mediaType === 'image'" @click="deleteImg1(index)" class="deleteBtn">X</span>
                            <!-- <img v-gallery:groupName v-if="item.mediaType === 'image'" :src="item.src" /> -->
                            <mt-progress v-if="!item.upFlag" :value="item.progress" :bar-height="1">
                                <div slot="end">{{item.progress}}%</div>
                            </mt-progress>
                            <div class="showImgCase">
                                <!-- <img :src="item.src" v-if="item.mediaType === 'image'"> -->
                                <!-- <img :src="`${item.src.replace(/cos.ap-guangzhou/g, 'picgz')}${compressHead}`" :large="item.src"  preview="patient1" preview-text=""> -->
                                <img :src="`${item.src.replace(/cos.ap-guangzhou/g, 'picgz')}${compressHead}`" @click="readImgInfo(imgList, item.src, 'src')" >
                            </div>
                            
                        </li>
                        <li class="imgListCase" v-if="item.mediaType === 'image'&&orderId!=0 && !item.isDelete"  v-for="(item, index) in attach1" :key="index">
                            <span @click="deleteImg(index)" class="deleteBtn">X</span>
                            <mt-progress v-if="!item.upFlag" :value="item.progress" :bar-height="2">
                                <div slot="end">{{item.progress}}%</div>
                            </mt-progress>
                            <!-- <img v-gallery:groupName v-if="item.mediaType === 'image'" :src="item.message" /> -->
                            <div class="showImgCase">
                                <!-- <img :src="item.message" v-if="item.mediaType === 'image'"> -->
                                <!-- <img :src="item.message" preview="patient1" preview-text=""> -->
                                <!-- <img :src="`${item.message.replace(/cos.ap-guangzhou/g, 'picgz')}${compressHead}`" :large="item.message"  preview="patient1" preview-text=""> -->
                                <img :src="`${item.message.replace(/cos.ap-guangzhou/g, 'picgz')}${compressHead}`" @click="readImgInfo(attach1, item.message, 'message')" >
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
                        <li class="imgListCase"  v-if="orderId===0 && item.mediaType != 'image'"  v-for="(item, index) in imgList" :key="index">
                            <span @click="deleteImg1(index)" class="deleteBtn">X</span>
                            <img @click="videoClick(true, item.message)" src="../../assets/image/videoPlay.jpg" />
                        </li>
                        <li class="imgListCase" v-if="item.mediaType != 'image' && orderId!=0 && !item.isDelete"  v-for="(item, index) in attach1" :key="index">
                            <span @click="deleteImg(index)" class="deleteBtn">X</span>
                            <img @click="videoClick(true, item.message)" src="../../assets/image/videoPlay.jpg" />
                        </li>
                    </ul>
                </div>
            </div> -->
            <div v-if="orderId===0" class="okCase">
                <div @click="getWxPay" class="okBtn">
                    <mt-button type="primary" class="mint-button--large">提交并支付</mt-button>
                </div>
            </div>
            <!-- <div v-if="orderId===0" class="okCase">
                <div @click="getWxPay" class="okBtn">
                    <mt-button type="primary" class="mint-button--large">支付测试</mt-button>
                </div>
            </div> -->
            <div v-if="orderId!=0" class="okCase">
                <div @click="submitConsult(false)" class="okBtn">
                    <mt-button type="primary" class="mint-button--large">保存</mt-button>
                </div>
            </div>
        </div>
        <mt-picker :slots="dataList" ref="picker" :showToolbar="true" v-show="show">
                <div @click="selectIllness(1)" class="slots-no">取消</div>
                <div @click="getPickerValue(1)" class="slots-ok">确认</div>
        </mt-picker>
        <mt-picker :slots="dataList1" ref="picker1" :showToolbar="true" v-show="show1">
            <div @click="selectIllness(2)" class="slots-no">取消</div>
            <div @click="getPickerValue(2)" class="slots-ok">确认</div>
        </mt-picker>
        <!-- <div v-if="videoShow" class="videoLook">
            <div class="videoBox">
                <video ref="videoSelf" controls :src="videoSrc" autoplay muted></video>
            </div>
            <div class="videoBtn">
				<mt-button @click="videoClick(false)" type="primary" class="mint-button--large">关闭</mt-button>
			</div>
        </div> -->
    </div>
</template>

<script>
import { Toast, MessageBox } from 'mint-ui';
import { verifyPrice, timestamp, dataURLtoBlob, fontVery } from 'assets/js/common.js'
import wx from 'weixin-js-sdk'
import Exif from 'exif-js'

export default {
    data () {
        return {
            id: 0,
            newId: 0,
            id1: 0,
            orderId: 0,
            patientName: '',
            patientId: 0,
            iIInessType: '',
            title: '填写咨询内容',
            iIInessId: -1,
            videoSrc: '',
            videoShow: false,
            add: '',
            phoneNumber: '',
            patientCity: '',
            iIInessDescription: '',
            doctorDetail: {},
            doctorInfo: {},
            remnant: 500,
            maxReplyLength: 500,
            show: false,
            show1: false,
            serviceMoneyListId: 0,
            fileSize: 0,
            consultID: 0,
            imgSize: 8,
            imgCount: 9,
            videoSize: 50,
            videoCount: 3,
            fileFolder: '',
            fileFolder1: '',
            dataSlots: [],
            slotsArry: [],
            consiltList: [],
            sexArray: [],
            imgList: [],
            upParams: {},
            upParams1: {},
            filename: '',
            storeAs: '',
            file1: {},
            index: 0,
            price: 0,
            imgSum: 0,
            serviceType: '',
            serviceType1: '',
            clickFlag: true,
            allClick: true,
            sp_billno: '',
            slots: [
                {
                    flex: 1,
                    values: [],
                    className: 'slot1',
                    textAlign: 'center'
                }
            ],
            fileArray: [],
            consultationfile: [],
            attach1: [],
            clickTag: true,
            imgUrlHead: '',
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
        videoClick(flag, video) {
            this.videoShow = flag
            if (flag) {
                this.videoSrc = video
                this.$refs.videoSelf.play()
                this.$refs.videoSelf.pause()
                this.$refs.videoSelf.play()
            } 
        },
        descInput() {
            let txtVal = this.iIInessDescription.length;
            const iIInessDescription = this.iIInessDescription
            window.localStorage.setItem('iIInessDescription', iIInessDescription)
            this.remnant = this.maxReplyLength - txtVal;
            
            // if(this.remnant<=0) {
            //     alert(this.iIInessDescription.length)
            //     this.iIInessDescription = this.iIInessDescription.substring(0, 500)
            //     alert(this.iIInessDescription.length)
            // }
        },
        replyParameter () { // 提交追问参数
            this.instance.consultationReplyParameter({
            })
                .then((response) => {
                    let replyList = response.data.result.item
					this.maxReplyLength = replyList.maxReplyLength
                    this.remnant = replyList.maxReplyLength
                    let txtVal = this.iIInessDescription.length
                    this.remnant = this.maxReplyLength - txtVal
                })
                .catch((error) => {
                }) 
		},
        getPickerValue(nub) { // 选择咨询人或疾病类型
            if (nub === 1) { // 获取疾病类型
                this.show = !this.show
                this.iIInessType = this.$refs.picker.getValues()[0]                
                for (let i = 0 ;i < this.lableManages.length; i++) {
                    if(this.lableManages[i].lableName === this.iIInessType) {
                        this.iIInessId = this.lableManages[i].id
                        return
                    } else {
                        this.iIInessId = -1
                    }
                }
            } else { // 获取咨询人信息
                this.show1 = !this.show1
                const id = this.serviceMoneyListId
                const price = this.price
                const serviceType = this.serviceType1
                let slectValue = this.$refs.picker1.getValues()[0]
                if (slectValue === '添加咨询人') {
                    this.$router.push({ 
                        path: '/add-number',
                        query: {id, price, serviceType}
                    })
                }
                this.patientName = this.$refs.picker1.getValues()[0]
                for (let i = 0 ;i < this.consiltList.length; i++) {
                    if(this.consiltList[i].leaguerName === this.patientName) {
                        this.patientId = this.consiltList[i].id
                        this.phoneNumber = this.consiltList[i].phoneNumber
                        return
                    } else {
                        this.patientId = 0
                    }
                }
            }   
            
        },
        selectIllness(index) { // 呼出咨询人或疾病类型
            if (index === 1) {
                this.show = !this.show
            } else {
                this.show1 = !this.show1
            }
            
        },
        getConsultList() { // 获取咨询人列表
            this.instance.leaguerInfoList({
            })
                .then((response) => {
                    this.consiltList = response.data.result.item
                    const sexStatus = ['', '男', '女']
                    for(var j = 0;j < this.consiltList.length;j++ ) {
                        this.sexArray.push(this.consiltList[j].leaguerName)
                        const statusNub = this.consiltList[j].sex
                        this.consiltList[j].sex = sexStatus[statusNub]
                    }
                    this.sexArray.push('添加咨询人')
                    if(this.add === 'new') {
                        const length = this.sexArray.length -2
                        this.patientName = this.sexArray[length]
                        for (let i = 0 ;i < this.consiltList.length; i++) {
                            if(this.consiltList[i].leaguerName === this.patientName) {
                                this.patientId = this.consiltList[i].id
                                this.phoneNumber = this.consiltList[i].phoneNumber
                                return
                            } else {
                                this.patientId = 0
                            }
                        }
                    }
                })
                .catch((error) => {
                }) 

        },
        getDoctorInfo() { // 获取医生信息
            const id = this.id
            this.instance.yaeherDoctorById({
                id
            })
                .then((response) => {
					this.doctorInfo = response.data.result.item
                })
                .catch((error) => {
                }) 
        },
        getIllnessList() { // 获取疾病列表带其它
            const doctorID = this.id
            this.instance.doctorConsultationRelationList({
                doctorID
            })
                .then((response) => {
                    this.lableManages = response.data.result.item
                    for(var j = 0;j < this.lableManages.length;j++ ) {
                        this.slotsArry.push(this.lableManages[j].lableName)
                    }
                })
                .catch((error) => {
                }) 
        },
		consultationDetail () { // 获取咨询详情
            if (this.orderId === 0) return
            const id = this.orderId
            this.instance.consultationDetail({
				id
            })
                .then((response) => {
                    const recordDetail = response.data.result.item
                    this.recordDetail = recordDetail
					this.replys = recordDetail.replys
					this.doctorId = recordDetail.doctorID
					window.sessionStorage.setItem('doctorId', this.doctorId)
					const sex = recordDetail.sex
					const sexStatus = ['', '男', '女']
					this.sex = sexStatus[sex]
                    this.patientName = recordDetail.patientName
                    this.consultationfile = recordDetail.consultationfile
                    for (var i = 0; i < this.consultationfile.length; i ++) {
                        let obj = {
                            id: this.consultationfile[i].id,
                            filename: this.consultationfile[i].fileName,
                            mediaType: this.consultationfile[i].mediatype,
                            fileSize: this.consultationfile[i].fileSize,
                            message: this.consultationfile[i].message,
                            imgSum: i,
                            isDelete: false,
                            upFlag: true
                        }
                        this.attach1.push(obj)
                        console.log(this.attach1)
                    }
                    this.$previewRefresh()
                    this.patientId = recordDetail.patientID
                    this.phoneNumber = recordDetail.phoneNumber
                    this.patientCity = recordDetail.patientCity
                    this.iIInessDescription = recordDetail.iiInessDescription
                    this.iIInessType = recordDetail.iiInessType
                    this.iIInessId = recordDetail.iiInessId
                    
                })
                .catch((error) => {
                }) 
		},
        submitConsult(flag) { // 提交咨询
            let _this = this
            const iIInessType = this.iIInessType
            const iIInessId = this.iIInessId
            const serviceMoneyListId = this.serviceMoneyListId
            const doctorId = this.id
            const patientId = this.patientId
            const phoneNumber = this.phoneNumber
            const patientCity = this.patientCity
            const iIInessDescription = this.iIInessDescription
            let attach = new Array()
            if (flag) {
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
            } else {
                // let fontFlag = fontVery(iIInessDescription)
                // if (!fontFlag) return
                // this.clickFlag = false
                if (!this.allClick) {
                    Toast('图片正在上传中，请稍候！')
                    return
                }
                if(!this.clickTag) {
                    MessageBox('提示信息', '请不要重复提交')
                    return
                }
                this.clickTag = false
                // attach = this.attach1
                for (var i = 0; i < this.attach1.length; i ++) {
                    if (this.attach1[i].isDelete === true && this.attach1[i].id === 0) {
                    } else {
                        let obj = {
                            id: this.attach1[i].id,
                            filename: this.attach1[i].filename,
                            message: this.attach1[i].message,
                            mediaType: this.attach1[i].mediaType,
                            fileSize: this.attach1[i].fileSize,
                            isDelete: this.attach1[i].isDelete
                        }
                        attach.push(obj)
                    }
                    
                }
                console.log('attch1',attach)
            }
            const id = this.orderId
            
            this.$indicator.open({
                text: '提交中请稍候',
                spinnerType: 'fading-circle'
            })
            // let sp_billno = this.sp_billno
            this.upParams = {
                // sp_billno,
                iIInessType,
                iIInessId,
                serviceMoneyListId,
                doctorId,
                patientId,
                phoneNumber,
                patientCity,
                iIInessDescription,
                attach
            }
            console.log(this.upParams)

            this.upParams1 = {
                id,
                patientId,
                iIInessId,
                phoneNumber,
                patientCity,
                iIInessDescription,
                attach
            }
            console.log(this.upParams1)

            let imgList = this.imgList
            // if (imgList.length === 0) {
                if(_this.orderId === 0) {
                    _this.addRecord()
                } else {
                    _this.alterRecord()
                }
            // } else {
            //     for (var j = 0; j < imgList.length; j ++) {
            //         this.uploadCos(imgList[j].storeAs, imgList[j].file1, imgList[j].mediaType, j)
            //     }
            // }
            
            
        },
        addRecord() { // 新增咨询
            
            let upParams = this.upParams
            
            let _this = this
            this.instance.createConsultation(
                    upParams
                )
                    .then((response) => {
                        if (response.data.result.code === 200) {
                            // Toast('提交成功')
                            window.localStorage.setItem('iIInessDescription', '')
                            let result = response.data.result.item
                            this.newId = result.id
                            this.consultNumber = result.consultNumber
                            _this.$indicator.close()
                            this.toPayFor()
                            // let serviceType = _this.serviceType1
                            
                            
                            // console.log(id)
                            
                            // setTimeout(function() {
                            //     _this.$router.push({ path: '/record-detail' , query: {id}})
                            // }, 100)
                        }
                    })
                    .catch((error) => {
                        _this.$indicator.close()
                        _this.clickTag = true
                        console.log('提交咨询失败')
                    }) 
        },
        alterRecord() { // 修改咨询
            let upParams1 = this.upParams1
            let _this = this
            this.instance.updateConsultation(
                    upParams1
                )
                    .then((response) => {
                        if (response.data.result.code === 200) {
                            window.localStorage.setItem('iIInessDescription', '')
                            _this.$indicator.close()
                            Toast('修改成功')
                            _this.clickTag = true
                            setTimeout(function() {
                                // _this.$router.push({ path: '/user-record'})
                                _this.$router.go(-1)
                            }, 500)
                            
                        }
                        
                    })
                    .catch((error) => {
                        _this.$indicator.close()
                        _this.clickTag = true
                        console.log('提交咨询失败')
                    })
        },
        uploadImgOss(evt, index2) { // 选择图片
            
            let file = evt.target
            let _this = this
            const fileList = file.files
            if(fileList.length != 0) {
                this.$indicator.open({
                    text: '图片上传中，请稍候',
                    spinnerType: 'fading-circle'
                })
            }
            // alert(fileList.length)
            console.log(fileList)
            
            
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
                console.log(filename)
                let fileSize = file.files[0].size
                let src = ''
                // let src = `${this.imgUrlHead}${filename}`
                
                //去获取拍照时的信息，解决拍出来的照片旋转问题
                let Orientation
				Exif.getData(file1, function(){
					Exif.getAllTags(this)
					
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
                    
                    if (_this.orderId!=0) { // 判断是否有订单id，有即为详情
                        let imgSum = 0
                        let videoSum = 0
                        console.log('length',_this.attach1)
                        for (let n = 0; n < _this.attach1.length; n++) {
                            if(_this.attach1[n].mediaType === 'image' && !_this.attach1[n].isDelete) {
                                imgSum = imgSum + 1
                            }else if (!_this.attach1[n].isDelete && _this.attach1[n].mediaType === 'video') {
                                videoSum = videoSum + 1
                            }
                        }
                        _this.imgSum = _this.attach1.length-1
                        // let mediaType = index2
                        let obj1 = {
                                id: 0,
                                filename,
                                mediaType,
                                fileSize,
                                message: src,
                                imgSum: _this.attach1.length-1,
                                isDelete: false,
                                progress: 0,
                                upFlag: false
                            }

                            let obj = {
                                fileSize,
                                file1,
                                filename,
                                storeAs,
                                mediaType,
                                src,
                                imgSum: _this.attach1.length-1,
                                progress: 0,
                                upFlag: false
                            }
                        if(obj.mediaType === 'image') {
                            if (imgSum < _this.imgCount) {
                                console.log('添加了img')
                                // _this.attach1.push(obj1)
                                // _this.imgList.push(obj)
                                // console.log('imgList',_this.imgList)
                                // console.log('attach1',_this.attach1)
                                let result = this.result;
                                let img = new Image();
                                img.src = result;
                                img.onload = function () {
                                    if (!Orientation || Orientation === 1) {
                                        // obj.src1 = result;
                                    } else {
                                        let src1 = _this.compress(img, Orientation);
                                        obj.file1 = dataURLtoBlob(src1)
                                        file1 = dataURLtoBlob(src1)
                                    }
                                    
                                    _this.attach1.push(obj1)
                                    _this.imgList.push(obj)
                                    console.log('imgList',_this.imgList)
                                    console.log('attach1',_this.attach1)
                                }
                                
                            } else {
                                _this.$indicator.close()
                                Toast(`图片最多上传${_this.imgCount}个`)
                                return
                            }
                            
                            
                        } else {
                            if (videoSum < _this.videoCount) {
                                _this.attach1.push(obj1)
                                _this.imgList.push(obj)
                            } else {
                                Toast(`视频最多上传${_this.videoCount}个`)
                                return
                            }
                        }
                        
                        setTimeout(function(){
                            _this.uploadCos(storeAs, file1, mediaType, _this.attach1.length-1, filename)
                        }, 500)
                    } else {
                        let imgSum = 0
                        let imgSum1 = 0
                        let videoSum = 0
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
                        console.log(imgSum, 1)
                        if(obj.mediaType === 'image') {
                            if (imgSum1 < _this.imgCount) {
                                // _this.imgList.push(obj)
                                let result = this.result;
                                let img = new Image();
                                img.src = result;
                                img.onload = function () {
                                    if (!Orientation || Orientation === 1) {
                                        // obj.src1 = result;
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
                                return
                            }
                        } else {
                            if (videoSum < _this.videoCount) {
                                _this.imgList.push(obj)
                            } else {
                                Toast(`视频最多上传${_this.videoCount}个`)
                                return
                            }
                        }
                        setTimeout(function(){
                            _this.uploadCos(storeAs, file1, mediaType, imgSum, filename)
                        }, 500)
                    }
                    
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
                    if (_this.orderId!=0) {
                        _this.attach1[index].progress = progress
                        
                    } else {
                        _this.imgList[index].progress = progress
                    }
                    console.log(progress)
                }
            }, function(err, data) {
                if (err) {
                    _this.$indicator.close()
                    console.log(err)
                    Toast('图片上传失败')
                    // _this.$indicator.close()
                    // _this.clickTag = true
                    // _this.uploadCos(key, body, type, index)
                } else {
                    console.log(data)
                    _this.$indicator.close()
                    let src = `${_this.imgUrlHead}${filename}`
                    let flagAll = true
                    if (_this.orderId!=0) {
                        _this.attach1[index].message = src
                        let srcRrpact = src.replace(/cos.ap-guangzhou/g, 'picgz')
                        _this.attach1[index].upFlag = true
                        for (let i = 0; i < _this.attach1.length; i++) {
                            if (!_this.attach1[i].isDelete) {
                                flagAll = flagAll && _this.attach1[i].upFlag
                            }
                            
                        }
                    } else {
                        _this.imgList[index].src = src
                        let srcRrpact = src.replace(/cos.ap-guangzhou/g, 'picgz')
                        _this.imgList[index].upFlag = true
                        for (let i = 0; i < _this.imgList.length; i++) {
                            if (!_this.imgList[i].isDelete) {
                                flagAll = flagAll && _this.imgList[i].upFlag
                            }
                            
                        }
                    }
                    console.log('end', flagAll)

                    if (flagAll) {
                        // _this.clickFlag = true
                        _this.allClick = true
                    }
                } 
            })
        },
        deleteImg (index) { // 删除图片
            if (this.imgSum === this.attach1[index].imgSum) {
                this.$indicator.close()
                this.$refs.imgFile.value =''
            }
            
            this.attach1[index].isDelete = true
            let flagAll = true
            for (let i = 0; i < this.attach1.length; i++) {
                if (!this.attach1[i].isDelete) {
                    flagAll = flagAll && this.attach1[i].upFlag
                }
            }
            if (flagAll) {
                this.allClick = true
            }    
            console.log(1111111111,this.attach1)
            console.log(flagAll)
        },
        deleteImg1 (index) { // 删除图片
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
                    this.mediaType = paramsObject.mediaType
                    this.serviceType = paramsObject.type[0].code
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
                    this.region = uploadParams.region
                    this.secretId = uploadParams.secretId
                    this.secretKey = uploadParams.secretKey
                    if (index === 0) {
                        this.fileFolder = uploadParams.fileFolder
                        this.imgUrlHead = uploadParams.fileHeadName
                    } else {
                        this.fileFolder1 = uploadParams.fileFolder
                    }
                    
                  
                })
                .catch((error) => {
                }) 
        },
        getWxPay () { // 获取支付参数
            // this.clickFlag = false
            if (!this.allClick) {
                Toast('图片正在上传中，请稍候！')
                return
            }
            let _this = this
            const iIInessType = this.iIInessType
            const iIInessId = this.iIInessId
            const serviceMoneyListId = this.serviceMoneyListId
            const doctorId = this.id
            const patientId = this.patientId
            const phoneNumber = this.phoneNumber
            const patientCity = this.patientCity
            const iIInessDescription = this.iIInessDescription
            // 校验数组
            let verifyJson = [
                {
                    value: patientId,
                    msg: '咨询人不能为空'
                },
                {
                    value: iIInessId,
                    msg: '疾病类型不能为空'
                },
                {
                    value: phoneNumber,
                    msg: '电话号码不能为空'
                },
                {
                    value: patientCity,
                    msg: '地址不能为空'
                },
                {
                    value: iIInessDescription,
                    msg: '问题描述不能为空'
                },
            ]

            let verifyPriceFlg = verifyPrice(verifyJson)
            if (verifyPriceFlg === 1) return
            // let fontFlag = fontVery(iIInessDescription)
            // if (!fontFlag) return
            let mes = `请确保医生可以通过该号码联系到您：${phoneNumber}`
            if(this.serviceType1 === 'Phone') {
                MessageBox.confirm('', {
                    message: mes, 
                    title: '请确认接听电话', 
                    confirmButtonText: '确认', 
                    cancelButtonText: '去修改' 
                }).then(action => {
                    _this.payFun()
                },function(){
                    console.log('取消了');
                })
            } else {
                _this.payFun()
            }
            
        },
        payFun () {
            if(!this.clickTag) {
                MessageBox('提示信息', '请不要重复提交')
                return
            }
            this.clickTag = false
            let _this = this
            this.submitConsult(true)
            // let productId = this.serviceMoneyListId
            // this.instance.wXOAuthPay({
            //     productId
            // })
            //     .then((response) => {
            //         let paramsObject = response.data.result.item
            //         console.log(paramsObject)
            //         let appId = paramsObject.appid
            //         let timestamp = paramsObject.timeStamp
            //         let nonceStr = paramsObject.nonceStr
            //         let package1 = paramsObject.package
            //         let signType = 'MD5'
            //         let signature = paramsObject.paySign
            //         let paySign = paramsObject.paySign
            //         this.sp_billno = paramsObject.sp_billno
            //         let obj = {
            //             appId,
            //             timestamp,
            //             nonceStr,
            //             package1,
            //             signType,
            //             signature,
            //             paySign
            //         }
            //         wx.config({
            //             debug: false,
            //             appId,
            //             timestamp,
            //             nonceStr,
            //             signature,
            //             jsApiList: ['chooseWXPay']
            //         })
            //         wx.ready(function () {
            //             wx.chooseWXPay({ // 微信支付
            //                 timestamp,
            //                 nonceStr,
            //                 'package': package1,
            //                 signType,
            //                 paySign,
            //                 success: function (res) {
            //                     _this.submitConsult(true)
            //                 },
            //                 fail: function (err) {
            //                     console.log(err)
            //                     _this.clickTag = true
            //                 },
            //                 cancel: function (res) {
            //                     _this.clickTag = true
            //                 }
            //             })
            //         })
            //         wx.error(function (res) {
            //             _this.clickTag = true
            //             Toast('调用出错')
            //         })
                    
            //     })
            //     .catch((error) => {
            //         _this.clickTag = true
            //     }) 
        },
        toPayFor() { // 去支付
			let _this = this
			console.log('点了支付')
			let consultNumber = this.consultNumber
            this.instance.wXOAuthPay({
                consultNumber
            })
                .then((response) => {
                    let paramsObject = response.data.result.item
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
                                        _this.$router.push({ path: '/record-detail' , query: {id: _this.newId}})
                                    })
                                    .catch((error) => {
                                        _this.$router.push({ path: '/record-detail' , query: {id: _this.newId}})
                                    }) 
                                
                            },
                            cancel: function (res) {
                                // Toast('您取消了支付！')
                                _this.instance.wXOAuthPayProcessingRelease({
                                    consultNumber
                                })
                                    .then((response) => {
                                        _this.$router.push({ path: '/record-detail' , query: {id: _this.newId}})
                                    })
                                    .catch((error) => {
                                        _this.$router.push({ path: '/record-detail' , query: {id: _this.newId}})
                                    }) 
                            }
                        })
                    })
                    wx.error(function (res) {
                        _this.$router.push({ path: '/record-detail' , query: {id: _this.newId}})
                    })
                    
                })
                .catch((error) => {
                    _this.$router.push({ path: '/record-detail' , query: {id: _this.newId}})
                }) 
        },
        submutOrder() { // 提交支付订单
			// const consultNumber = this.consultNumber
			// const sp_billno = this.sp_billno
            const serviceType = this.serviceType1
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
            //     })    
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
    computed: {
        dataList() {
            let dataSlots = [
                {
                    flex: 1,
                    values: this.slotsArry,
                    className: 'slot1',
                    textAlign: 'center'
                }
            ]
            return dataSlots
        },
        dataList1() {
            let dataSlots1 = [
                {
                    flex: 1,
                    values: this.sexArray,
                    className: 'slot2',
                    textAlign: 'center'
                }
            ]
            return dataSlots1
        }
    },
    mounted () {
        this.consultationDetail()
        this.replyParameter()
    },
    created () {
        this.id1 = JSON.parse(window.sessionStorage.getItem('doctorIdU'))
        this.id = JSON.parse(window.sessionStorage.getItem('doctorId'))
        this.price = this.$route.query.price
        this.serviceType1 = this.$route.query.serviceType
        
        this.add = this.$route.query.add
        let orderId = this.$route.query.orderId
        if (!orderId) {
            this.orderId = 0
            this.title = '填写咨询内容'
        } else {
            this.orderId = parseInt(orderId)
            this.title = '修改咨询'
        }
        let iIInessDescription = window.localStorage.getItem('iIInessDescription')
        if (iIInessDescription) {
            this.iIInessDescription = iIInessDescription
        }
        this.serviceMoneyListId = parseInt(this.$route.query.id)
        this.getDoctorInfo()
        this.getConsultList()
        this.getIllnessList()
        this.getTypeParams()
        
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.consultation {
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
    .answer {display: flex; font-size: $font-l; padding: 10px 15px; margin: 10px 0; background: $color-wfont;}
    .answer > div {flex: 1;}
    .doctorAn { text-align: right;}
    .answer-name {line-height: 20px;}
    .doctorAn img, .doctorAn span {display: inline-block; vertical-align: middle;}
    .picker {position: absolute; bottom: 0; left: 0; right: 0; background: $color-wfont; z-index: 10;}
    .upload-row {position: relative; background: $color-wfont; padding: 10px 10px;}
    .uploadImgList { padding: 10px 5px; display: flex; flex-wrap:wrap;}
    .uploadImgList li {margin: 10px 5px 0; padding: 0; border-bottom: none; width: 100px; height: 100px;}
    .uploadImgList li .showImgCase{width: 100px; height: 100px; overflow: hidden; position: relative; border-radius: 3px;}
    .uploadImgList li img {display: block;}
    .uploadImg {padding-bottom: 15px; overflow: hidden;}
    .uploadImg > label {float: left; margin-right: 10px; position: relative;}
    .uploadImg input{visibility: hidden; position: absolute; display: block; width: 100%; height: 100%;}
    .uploadImgCase {width: 50px; height: 50px; background: url(../../assets/image/add-pic.png) no-repeat;background-size: 100%;}
    .uploadVideoCase {width: 50px; height: 50px; background: url(../../assets/image/add-pic.png) no-repeat;background-size: 100%;}
    .okCase {padding: 0 10px 20px 10px; background: $color-wfont;}
    .illness {background-color: $color-wfont; padding: 0 0 0 10px;}
    .illness-case {display: flex; padding: 10px 0;
    }
    .illness-label {width: 105px;}
    .illness-label::after {content: '*'; color: $color-red;vertical-align: bottom; line-height: 18px; padding-left: 1px;}
    .mint-cell-text::after {content: '*'; color: $color-red;vertical-align: bottom; line-height: 10px; padding-left: 1px;}
    .patient-info .mint-cell:nth-last-child(1) .mint-cell-text::after {content: '';}
    .patient-info {
        position: relative;
        padding-bottom: 15px;
        background-color: $color-wfont;
        .wordsNumb {
            position: absolute;
            right: 10px;
            bottom: 0;
            font-size: $font-l;
            color: #999;
        }
    }
    .imgListCase {
        position: relative;
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
            position: absolute;
            top: 50%;
            left: 50%;
            display: block;
            height: 100%;
            // width: 100%;
            // min-width: 100%;
            // min-height: 100%;
            transform:translate(-50%,-50%);
            border-radius: 5px;
        }
        video{
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
    
}

</style>