<template>
    <div class="avatar padding-top">
        <mt-header fixed title="我的头像">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
            <p class="smallHint">小头像：建议采用正面半身照，头面部清晰，背景为浅色，如白色、淡灰色，避免红色、蓝色等。 <br/>个人主页照片：建议采用正面半身照，头面部清晰，背景整洁，无杂物，背景色为浅色，避免深色。</p>
            <div class="avatarCase">
                <div class="avatarList">
                    <h2>小头像</h2>
                    <p>用于聊天展示</p>
                    <div class="avatarAlign">
                        <div class="idCardCase">
                            <img v-if="!src1" src="../../assets/image/logo.jpg" />
                            <img v-if="src1" :src="src1" />
                            <label for="idCard1">
                                <input id="idCard1" accept="image/*" type="file" v-on:change="uploadImgOss($event, 3)"/>
                            </label>
                        </div>
                        <p>点击图片上传</p>
                    </div>
                </div>
                <div class="avatarList">
                    <h2>个人主页背景</h2>
                    <p>用于个人主页展示</p>
                    <div class="avatarAlign avatarIndex">
                        <div class="idCardCase">
                            <img v-if="!src2" src="../../assets/image/logo.jpg" />
                            <img v-if="src2" :src="src2" />
                            <label for="idCard2">
                                <input id="idCard2" accept="image/*" type="file" v-on:change="uploadImgOss($event, 4)"/>
                            </label>
                        </div>
                        <p>点击图片上传</p>
                    </div>
                </div>
            </div>

            <div v-if="show" class="cropperBox">
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
                <div class="buttomCase">
                    <div>
                        <mt-button @click="cropperClose" type="default" class="mint-button--large">取消</mt-button>
                    </div>
                    <div>
                        <mt-button @click="arrowLeft" type="default" class="mint-button--large">左旋转</mt-button>
                    </div>
                    <div>
                        <mt-button @click="arrowRight" type="primary" class="mint-button--large">右旋转</mt-button>
                    </div>
                    <div>
                        <mt-button @click="cropperOpen" type="primary" class="mint-button--large">确认</mt-button>
                    </div>
                    
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { Toast } from 'mint-ui';
import { VueCropper }  from 'vue-cropper' 
import { verifyPrice, timestamp, compress } from 'assets/js/common.js'
import Exif from 'exif-js'
export default {
    components: { 
      VueCropper 
    }, 
    data () {
        return {
            id: 0,
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
            documentsUse: '',
            show: false,
            type1: 3,
            example2: {
                img: '',
                info: true,
                size: 1,
                outputType: 'jpeg',
                canScale: false,
                autoCrop: true,
                // 只有自动截图开启 宽度高度才生效
                autoCropWidth: 300,
                autoCropHeight: 210,
                // 开启宽度和高度比例
                fixed: true,
                fixedNumber: [2, 2]
            }
        }
    },
    methods: {
        cropperClose () {
            this.show = false
            this.getUserInfo()
        },
        cropperOpen () { // 裁剪
            // this.$refs.cropper.getCropData((data) => {
            //     if (this.type1 === 3) {
            //         this.src1 = data
            //     } else {
            //         this.src2 = data
            //     }
                
            // console.log(data)  
            // })
            this.$refs.cropper.getCropBlob((data) => {
                this.file1 = data
                let index = this.index
                this.getTypeParams(index)
            })
            this.show = false
        },
        arrowLeft () {
            this.$refs.cropper.rotateLeft()
        },
        arrowRight() {
            this.$refs.cropper.rotateRight()
        },
        uploadImgOss(evt, index) { // 选择图片
            this.type1 = index
            let file = evt.target
            let _this = this
            
            let fileName = file.files[0].name
            this.fileSize = file.files[0].size
            let file1 = file.files[0]
            if (this.fileSize >= 8*1024*1024) {
                Toast(`图片不能超过8M`)
                return
            }
            let Orientation
            //去获取拍照时的信息，解决拍出来的照片旋转问题
            // Exif.getData(file1, function(){
            //     Exif.getAllTags(this)
            //     Orientation = Exif.getTag(this, 'Orientation')
            // });
            let imgType = /^image\//
            let videoType = /^video\//
            if (imgType.test(file1.type)) {
            } else {
                return
            }
            let suffix = fileName.substr(fileName.indexOf('.'))
            let obj = timestamp()  
            this.filename = `wx-${obj}${suffix}`
            console.log(this.filename)
            let reader = new FileReader()
            reader.readAsDataURL(file1)
            
            reader.onload = function () {
                 _this.example2.img = this.result
                if (index === 3) {
                   _this.example2.fixedNumber = [200, 200]
                } else {
                    _this.example2.fixedNumber = [375, 210]
                }
                _this.show = true
                _this.file1 = file1
                _this.index = index
                // let img = new Image();
                // img.src = result;
                // let imgUrl = ''
                // img.onload = function () {
                //     if (!Orientation || Orientation === 1) {
                //         imgUrl = result;
                //     } else {
                //         imgUrl = _this.compress(img, Orientation);
                //         _this.example2.fixedNumber = [200, 200]
                //     }
                //     if (index === 3) {
                //         _this.example2.img = imgUrl
                //     } else {
                //         _this.example2.img = imgUrl
                //         _this.example2.fixedNumber = [375, 210]
                //     }
                //     _this.show = true
                //     _this.file1 = file1
                //     _this.index = index
                // }    
            }
        },
        compress (img, Orientation) {
            let canvas = document.createElement("canvas");
            let ctx = canvas.getContext('2d');
            ctx.fillStyle = "#fff";
            ctx.fillRect(0, 0, canvas.width, canvas.height);
            if(Orientation != "" && Orientation != 1){
                switch(Orientation){
                    case 6://需要顺时针（向左）90度旋转
                        this.rotateImg(img,'left',canvas);
                        
                        break;
                    case 8://需要逆时针（向右）90度旋转
                        this.rotateImg(img,'right',canvas);
                        break;
                    case 3://需要180度旋转
                        this.rotateImg(img,'right',canvas);//转两次
                        this.rotateImg(img,'right',canvas);
                    break;
                }
            }   
            let ndata = canvas.toDataURL('image/jpeg', 1) 
            return ndata;
        },
        rotateImg (img, direction,canvas) {
            //最小与最大旋转方向，图片旋转4次后回到原方向   
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
        getTypeParams (index) { // 获取type参数
            this.instance.tencentCosAccessTokenType({
            })
                .then((response) => {
                    this.mediaType = response.data.result.item.mediaType[0].code
                    this.serviceType = response.data.result.item.type[index].code
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
                    let storeAs = `${this.fileFolder}/${this.filename}` 
                    this.storeAs = storeAs
                    this.uploadCos(this.storeAs, this.file1)
                    
                })
                .catch((error) => {
                }) 
        },
        uploadCos (key, body) { // 上传图片
            // 初始化实例
            let _this = this
            var COS = require('cos-js-sdk-v5')
            var cos = new COS({
                SecretId: this.secretId,
                SecretKey: this.secretKey,
            })
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
                    Toast('上传失败，可刷新后再次上传')
                    console.log(err)
                } else {
                    console.log(data)
                    _this.authTypeSelect()
                }

                
            })
        },
        authTypeSelect () { // 上传背景图片
            const id = this.id
            const userImageFile = this.storeAs
            const userImage = this.storeAs
            if(this.index === 3) {
                console.log('上传了头像')
                this.instance.updateYaeherUser({
                    id,
                    userImage
                })
                    .then((response) => {
                        if (response.data.result.code === 200) {
                            Toast('上传成功')
                            this.getUserInfo()
                        }
                        
                    })
                    .catch((error) => {
                    }) 
            } else {
                this.instance.updateYaeherDoctorD({
                    id,
                    userImageFile
                })
                    .then((response) => {
                        if (response.data.result.code === 200) {
                            Toast('上传成功')
                            this.getUserInfo()
                        }
                    })
                    .catch((error) => {
                    }) 
            }
            
        },
        getUserInfo() { // 请求个人信息
            this.instance.yaeherDoctorD({
            })
                .then((response) => {
                    const userdetail = response.data.result.item
                    this.src1 = userdetail.userImage
                    this.src2 = userdetail.userImageFile

                })
                .catch((error) => {
                }) 
        }
        
    },
    mounted () {
        this.getUserInfo()
    },
    created () {
        this.id = parseInt(window.sessionStorage.getItem('userId'))
    }
}
</script>
<style lang="scss">
@import "~assets/sass/base.scss";

.avatar {
    .cropperBox {
        position: fixed;
        left: 0;
        bottom: 0;
        right: 0;
        top: 0;
        z-index: 2;
        .buttomCase {
            position: absolute;
            bottom: 0;
            right: 0;
            left: 0;
            height: 50px;
            display: flex;
            div {
                flex: 1;
                padding: 5px 10px;
                .mint-button {
                    font-size: $font-m;
                }
            }
        }
    }
    .smallHint {
        padding: 10px 15px;
        font-size: $font-m;
    }
    .avatarCase {
        background-color: $color-wfont;
        padding: 10px 15px;
        .avatarList {
            padding-bottom: 10px;
            h2 {
                font-weight: 500;
                font-size: 20px;
                line-height: 30px;
                padding: 5px 0;
            }
            p {
                font-size: $font-l;
                color: $color-bfont;
            }
            .avatarAlign {
                display: flex;
                align-items: center;
                padding: 20px 0;
                flex-direction: column;
                .idCardCase {
                    position: relative;
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
                    img{
                        width: 150px;
                        height: 150px;
                        border-radius: 10px;
                    }
                }
                
                p {
                    padding: 10px 0;
                }
            }
            .avatarIndex {
                .idCardCase {
                    img {
                        width: 250px;
                        height: 140px;
                    }
                }
            }
        
        }
        
    }
}

</style>