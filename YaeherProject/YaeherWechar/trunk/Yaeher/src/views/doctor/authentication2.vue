<template>
    <div class="authentication2 padding-top">
        <mt-header fixed title="怡禾认证2/3">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
            <mt-button v-if="show"  slot="right">
                <router-link to="/authentication3" class="right-white">下一步</router-link>
            </mt-button> 
        </mt-header>
        <div class="content">
            
            <h4>实名认证</h4>
            <div class="upload-case">
                <p>拍照示例</p>
                    <img src="../../assets/image/idCard1.png" />
                <p class="blod-p">*注 拍摄时请确保身份证边框完整，字体清晰，亮度均匀</p>
            </div>
            <div class="upload-case">
                <p>请确保姓名、身份证号等信息清晰可见</p>
                <div class="idCardCase">
                    <img v-if="!src1" src="../../assets/image/idCard1.png" />
                    <img v-if="src1" :src="src1" />
                    <label for="idCard1">
                        <input id="idCard1" accept="image/*" type="file" v-on:change="uploadImgOss($event, 0)"/>
                    </label>
                </div>
                
                <p class="blod-p">上传身份证正面</p>
            </div>
            <div class="upload-case">
                <p>请确有效期限等信息清晰可见</p>
                <div class="idCardCase">
                    <img v-if="!src2" src="../../assets/image/idCard2.png" />
                    <img v-if="src2" :src="src2" />
                    <label for="idCard2">
                        <input id="idCard2" accept="image/*" type="file" v-on:change="uploadImgOss($event, 1)"/>
                    </label>
                </div>
                <p class="blod-p">上传身份证背面</p>
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
            show: false,
            fileSize: 0,
            filename: '',
            storeAs: '',
            file1: {},
            index: 0,
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
            documentsUse: ''
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
            
            // let reader = new FileReader()
            // reader.readAsDataURL(file1)
            
            // reader.onload = function () {
            //     if (index === 0) {
            //         _this.src1 = this.result
            //     } else {
            //         _this.src2 = this.result
            //     }
            // }
            
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
                    this.serviceType = response.data.result.item.type[7].code
                    this.typeDetail = response.data.result.item.typeDetail[index].code
                    this.documentsUse = response.data.result.item.documentDetail[0].code
                    this.getUploadParams()
                })
                .catch((error) => {
                }) 
        },
        getUploadParams () { // 获取上传参数
            const serviceType = this.serviceType
            const mediaType = this.mediaType
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
                    this.uploadCos(this.storeAs, this.file1, this.index)
                })
                .catch((error) => {
                }) 
        },
        putImgUpload () { // 上传图片参数
            const fileType = this.serviceType
            const typeDetail = this.typeDetail
            const documentsUse = this.documentsUse
            const address = this.storeAs
            let _this = this
            let id = 0
            if ('idcardup' === typeDetail) {
                id = this.src1Id
            } else if ('idcarddown' === typeDetail) {
                id = this.src2Id
            }
            this.instance.doctorFileApplyD({
                id,
                fileType,
                typeDetail,
                documentsUse,
                address
            })
                .then((response) => {
                    if (response.data.result.code === 200) {
                        _this.getIdcardImg()
                        Toast('图片上传完成')
                    }
                })
                .catch((error) => {
                }) 
        },
        getIdcardImg () { // 获取身份证图片
            const documentsUse = 'register'
            const fileType = 'idcard'
            this.instance.doctorFileApplyListD({
                fileType,
                documentsUse
            })
                .then((response) => {
                    let _this = this
                    let uploadParams = response.data.result.item
                    if (!uploadParams) {
                        uploadParams = []
                    }
                    console.log()
                    for (let i = 0; i < uploadParams.length; i++) {
                        if (uploadParams[i].typeDetail === 'idcardup') {
                            _this.src1 = uploadParams[i].address
                            _this.src1Id = uploadParams[i].id
                            console.log(_this.src1Id)
                        } else if (uploadParams[i].typeDetail === 'idcarddown') {
                            _this.src2 = uploadParams[i].address
                            _this.src2Id = uploadParams[i].id
                            console.log(_this.src2Id)
                        }
                    }
                    if (_this.src1Id * _this.src2Id > 0) {
                        this.show = true
                    }
                })
                .catch((error) => {
                }) 
        },
        
    },
    mounted () {
        this.getTypeParams()
        this.getIdcardImg()
    },
    created () {
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.authentication2 {
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
}
</style>