<template>
    <div class="authenticationClinic padding-top">
        <mt-header fixed title="科室认证">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
            
            <h4>资格认证</h4>
            <div class="upload-case">
                <p>拍照示例</p>
                <img src="../../assets/image/idCard3.jpg" />
                <p class="blod-p">*注 拍摄时请确保证件边框完整，字体清晰，亮度均匀</p>
            </div>
            <div class="upload-case">
                <p>资格证和执业证或者胸牌</p>
                <div class="idCardCase">
                    <img v-if="!src1" src="../../assets/image/idCard3.jpg" />
                    <img v-if="src1" :src="src1" />
                    <label for="idCard1">
                        <input id="idCard1" accept="image/*" type="file" v-on:change="uploadImgOss($event, 3)"/>
                    </label>
                </div>
                
                <p class="blod-p">上传资格证</p>
            </div>
            <div class="upload-case">
                <p>资格证和执业证或者胸牌</p>
                <div class="idCardCase">
                    <img v-if="!src2" src="../../assets/image/idCard3.jpg" />
                    <img v-if="src2" :src="src2" />
                    <label for="idCard2">
                        <input id="idCard2" accept="image/*" type="file" v-on:change="uploadImgOss($event, 2)"/>
                    </label>
                </div>
                <p class="blod-p">上传执业证或者胸牌</p>
            </div>
            <div v-if="show" class="auth-ok">
                <a @click="submitAuthAll" class="okBtn">
                    <mt-button type="primary" class="mint-button--large">提交认证</mt-button>
                </a>
            </div>
            
        </div>
    </div>
</template>

<script>
import { Toast } from 'mint-ui';
import { verifyPrice, timestamp } from 'assets/js/common.js'
export default {
    data () {
        return {
            id: 0,
            show: false,
            fileSize: 0,
            filename: '',
            storeAs: '',
            file1: {},
            index: 0,
            clinicID: 0,
            secretId: '',
            secretKey: '',
            bucket: '',
            region: '',
            mediaType: '',
            serviceType: '',
            typeDetail: '',
            src1: '',
            src2: '',
            typeDetailList: [],
            src1Id: 0,
            src2Id: 0,
            qualificationcertificate: '',
            certificateofpractice: ''
        }
    },
    methods: {
        uploadImgOss(evt, index) { // 选择图片
            let file = evt.target
            let _this = this
            let fileName = file.files[0].name
            this.fileSize = file.files[0].size
            let file1 = file.files[0]
            let imgType = /^image\//
            let videoType = /^video\//
            console.log(imgType.test(file1.type))
            console.log(index)
            if (imgType.test(file1.type)) {
                console.log('上传了图片')
            } else {
                console.log('请上传正确文件')
                return
            }
            let suffix = fileName.substr(fileName.indexOf('.'))
            let obj = timestamp()  
            this.filename = `wx-${obj}${suffix}`
            
            let reader = new FileReader()
            reader.readAsDataURL(file1)
            
            reader.onload = function () {
                if (index === 3) {
                    _this.src1 = this.result
                } else {
                    _this.src2 = this.result
                }
            }
            
            this.file1 = file1
            this.index = index
            this.getTypeParams(index)
            
        },
        uploadCos (key, body, type) { // 上传图片
            // 初始化实例
            let _this = this
            var COS = require('cos-js-sdk-v5')
            var cos = new COS({
                SecretId: this.secretId,
                SecretKey: this.secretKey,
            })
            console.log('bucket', _this.bucket)
            console.log('regin', _this.region)
            cos.putObject({
                Bucket: _this.bucket, /* 必须 */
                Region: _this.region,    /* 必须 */
                Key: key,              /* 必须 */
                Body: body, // 上传文件对象
                onProgress: function(progressData) {
                    console.log('上传进度', progressData.percent*100)
                }
            }, function(err, data) {
                if (err) {
                    console.log(err)
                } else {
                    console.log(data)
                    _this.putImgUpload()
                }

                
            })
        },
        getTypeParams (index) { // 获取type参数
            this.instance.tencentCosAccessTokenType({
            })
                .then((response) => {
                    this.mediaType = response.data.result.item.mediaType[0].code
                    this.serviceType = response.data.result.item.type[8].code
                    this.typeDetail = response.data.result.item.typeDetail[index].code
                    this.documentsUse = response.data.result.item.documentDetail[0].code
                    console.log('typeDetail1', this.typeDetail)
                    const typeDetailList = response.data.result.item.typeDetail
                    if (typeDetailList) {
                        this.typeDetailList = typeDetailList
                    } else {
                        this.typeDetailList = []
                    }
                    this.getUploadParams()
                })
                .catch((error) => {
                }) 
        },
        getUploadParams () { // 获取上传参数
            const serviceType = this.serviceType
            const mediaType = this.mediaType
            const index = this.index
            this.instance.tencentCosAccessToken({
                mediaType,
                serviceType
            })
                .then((response) => {
                    const uploadParams = response.data.result.item
                    this.bucket = uploadParams.bucket
                    this.fileFolder = uploadParams.fileFolder
                    this.region = uploadParams.region
                    this.secretId = uploadParams.secretId
                    this.secretKey = uploadParams.secretKey
                    console.log(this.fileFolder)
                    console.log(this.bucket)
                    let storeAs = `${this.fileFolder}/${this.typeDetail}/${this.filename}` 
                    console.log(storeAs)
                    this.storeAs = storeAs
                    if (index === 3) {
                        this.qualificationcertificate = this.storeAs
                    } else {
                        this.certificateofpractice = this.storeAs
                    }
                    this.uploadCos(this.storeAs, this.file1, this.index)
                })
                .catch((error) => {
                }) 
        },
        putImgUpload () { // 上传图片参数
            const fileType = this.serviceType
            const mediaType = this.mediaType
            const typeDetail = this.typeDetail
            const documentsUse = this.documentsUse
            const address = this.storeAs
            let id = 0
            this.instance.doctorFileApplyD({
                id,
                documentsUse,
                fileType,
                typeDetail,
                address
            })
                .then((response) => {
                    
                    if(this.src1 && this.src2) {
                        this.show = true
                    }
                })
                .catch((error) => {
                }) 
        },
        submitAuthAll () { // 提交认证
            const clinicID = this.clinicID
            const applyType = this.applyType
            const qualificationcertificate = this.qualificationcertificate
            const certificateofpractice = this.certificateofpractice
            const id = this.id
            let params = {qualificationcertificate,certificateofpractice}
            console.log(params)
            // return
            this.instance.doctorClinicApplyD({
                id,
                clinicID,
                applyType,
                qualificationcertificate,
                certificateofpractice
            })
                .then((response) => {
                    if (response.data.result.code === 200) {
                        Toast('提交成功，请耐心等待审核！')
                        this.$router.push({ 
                            path: '/administrative-office'
                        })
                    }
                    
                })
                .catch((error) => {
                }) 
        },
        replyParameter () { // 获取审核参数
            this.instance.yaeherPatientParameterListD({
                Type: 'ConfigPar',
                SystemCode: 'DocumentsUse'
            })
                .then((response) => {
                    const paramsType = response.data.result.item
                    this.applyType = paramsType[2].code
                })
                .catch((error) => {
                }) 
        },
        authDetail () { // 获取认证详情
            this.instance.doctorClinicApplyByIdD({
                id: this.id
            })
                .then((response) => {
                    const authenDetail = response.data.result.item
                    this.src2 = authenDetail.certificateofpractice
                    this.src1 = authenDetail.qualificationcertificate
                    this.certificateofpractice = authenDetail.certificateofpractice.split('.com/')[1]
                    this.qualificationcertificate = authenDetail.qualificationcertificate.split('.com/')[1]
                })
                .catch((error) => {
                }) 
        },
    },
    mounted () {
        this.replyParameter()
        if (this.id !=0) {
            this.authDetail()
        }
        
    },
    created () {
        this.clinicID = this.$route.query.clinicID
        const id = this.$route.query.id
        if (id) {
            this.id = parseInt(id)
        }
        console.log(this.id)
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.authenticationClinic {
    h4 {
        font-weight: 600;
        font-size: $font-xxl;
        text-align: center;
        line-height: 40px;
        background: $color-wfont;
        margin-bottom: 5px;
    }
    .upload-case {
        background: $color-wfont;
        padding: 20px;
        text-align: center;
        margin-bottom: 5px;
        .idCardCase {
            position: relative;
            width: 250px;
            margin: 0 auto;
            label {
                display: block;
                position: absolute;
                top: 0;
                left: 0;
                right: 0;
                bottom: 0;
                z-index: 2;
                input {
                    visibility: hidden;
                }
            }
            img {
                width: 250px;
                max-height: 170px; 
                border-radius: 5px;
            }
        }
        img {
            width: 250px;
            max-height: 170px; 
            border-radius: 5px;
        }
        p {
            color: $color-bfont;
            font-size: $font-m;
            line-height: 30px;
        }
        .blod-p {
            font-size: $font-l;
        }
    }
    .auth-ok {
        margin: 10px;
    }
}
</style>