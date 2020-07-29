<template>
    <div class="qrCode padding-top">
        <mt-header fixed title="我的二维码">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content relative">
            <!-- <div class="qrCase">
				<div class="qrTop">
					<img src="../../assets/image/lu.png" />
					<div>
						<p>黄彪</p>
						<p>怡禾健康</p>
					</div>
				</div>
				<div class="qrImg" ref="box">
                    <img src="../../assets/image/lu.png" alt="分享背景图">
                    <div id="qrcode"></div>
				</div>
			</div> -->
            <div v-if="imgUrl" class="share-img">
                <img :src="imgUrl">
            </div>
            <div class="creat-img" ref="box">
                <img class="img1" src="../../assets/image/qrBg.png"  alt="分享背景图">
                <img class="img2" :src="avatar">
                <div class="imgText">
                    <p class="imgName">{{userName}}</p>
                    <p>{{hospitalName}}</p>
                </div>
                <div id="qrcode" class="qrcode"></div>
            </div>
        </div>
    </div>
</template>

<script>
// import QRCode from 'qrcodejs2'
import { qrcanvas } from 'qrcanvas';
import html2canvas from 'html2canvas';
export default {
     data () {
        return {
            imgUrl: '',
            userName: '',
            hospitalName: '',
            avatar: '',
        }
    },
     watch:{
        imgUrl(val,oldval){
            //监听到imgUrl有变化以后 说明新图片已经生成 隐藏DOM
            this.$refs.box.style.display = "none";
        }
    },
    methods: {
        base64ToBlob(code) {
            let parts = code.split(';base64,');
            let contentType = parts[0].split(':')[1];
            let raw = window.atob(parts[1]);
            let rawLength = raw.length;
            let uInt8Array = new Uint8Array(rawLength);
            for (let i = 0; i < rawLength; ++i) {
            uInt8Array[i] = raw.charCodeAt(i);
            }
            return new Blob([uInt8Array], {type: contentType});
        },
        getBase64Image(img) {
            var canvas = document.createElement("canvas");
            canvas.width = img.width;
            canvas.height = img.height;
            var ctx = canvas.getContext("2d");
            ctx.drawImage(img, 0, 0, img.width, img.height);
            var ext = img.src.substring(img.src.lastIndexOf(".")+1).toLowerCase();
            var dataURL = canvas.toDataURL("image/"+ext);
            return dataURL;
        }
    },
    mounted () {
        

    },
    created() {
        
        let id = window.sessionStorage.getItem('userId')
        let url = window.location.href
        let arrUrl = url.split("#")[0]
        let doctorUrl = `${arrUrl}#/doctor-detail-patient?id=${id}`
        this.userName = window.sessionStorage.getItem('userName')
        this.hospitalName = this.$route.query.hospitalName
        let avatar1 = window.sessionStorage.getItem('userImage')
        
        // var avatar1 = "http://thirdwx.qlogo.cn/mmopen/s9eVM20yUKIia79Xm53RdPBGJCrEdykImjQHl9ycMEXcfVdpVLNX0e8iazUo0PxLuy8ntWyUOxVGHC6QfPQ1XvZEaSWH3hfWWB/132"
        let that = this
        let img = new Image()
        img.crossOrigin = ''
        img.src = avatar1
        img.onload = function(){
            let baseUrl = that.getBase64Image(img)
            that.avatar = baseUrl
        }
        
        that.$nextTick(function () {
            var canvas1 = qrcanvas({
                data: decodeURIComponent(doctorUrl), //分享链接（根据需求来）
                size: 150 //二维码大小
            })
            document.getElementById("qrcode").innerHTML = ''
            document.getElementById('qrcode').appendChild(canvas1);
            that.$indicator.open({
                text: '正在生成图片...',
                spinnerType: 'fading-circle'
            });
            setTimeout(function(){
                html2canvas(that.$refs.box, {useCORS:true, logging: false}).then(function(canvas) {
                    that.imgUrl = canvas.toDataURL()
                    window.localStorage.setItem('imgUrl', that.imgUrl)          
                    setTimeout(()=>{
                        that.$indicator.close(); 
                        that.$toast({
                            message: '图片已生成，长按保存分享给你的好友吧',
                            position: 'middle',
                            duration: 3000
                        });                   
                    },2000)

                });
            }, 500)
        })
        
        
        
        
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.qrCode{
    .relative {
        position: absolute;
        top: 42px;
        left: 0;
        right: 0;
        bottom: 0;
        .share-img {
            position: absolute;
            top: 10px;
            bottom: 20px;
            left: 20px;
            right: 20px;
            border-radius: 10px;
            img {
                width: 100%;
                max-height: 100%;
                border-radius: 10px;
            }
        }
        .creat-img{
            position: absolute;
            top: 10px;
            right: 20px;
            left: 20px;
            bottom: 20px;
            img{
                z-index: 3;
                width: 100%;
                max-height: 100%;
            }
            .img2 {
                position: absolute;
                top: 50px;
                left:30px;
                z-index: 5;
                width: 80px;
                border-radius: 5px;
            }
            .qrcode{
                position: absolute;
                top: 305px;
                left: 50%;
                margin: 0 0 0 -75px;
                z-index: 5;
            }
            .imgText {
                position: absolute;
                top: 150px;
                left:30px;
                right: 50px;
                z-index: 5; 
                font-size: $font-m;
                color: $color-bfont;
                .imgName {
                    font-size: $font-xl;
                    color: $color-afont;
                    line-height: 30px;
                }
            }
        }

    }
}


</style>