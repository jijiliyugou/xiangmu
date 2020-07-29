<template>
    <div class="auditClinicDetail padding-top">
        <mt-header fixed title="科室审核详情">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">   
            <div class="doctorInfo">
                <div class="imgCase">
                    <img :src="userdetail.userImage" />
                </div>
                <div class="infoCase">
                    <p>{{userdetail.doctorName}}</p>
                    <p class="red-hint">申请的科室：{{userdetail.clinicName}}</p>
                    <p>{{userdetail.createdOn}}</p>
                </div>
            </div>
            <div class="border-top border-case">    
                <div class="flex auth-case">
                    <div class="doctor-auth">
                        <p>资格证</p>
                        <img v-gallery :src="userdetail.qualificationcertificate" />
                    </div>
                    <div class="doctor-auth">
                        <p>执业证</p>
                        <img v-gallery :src="userdetail.certificateofpractice"  />
                    </div>
                </div>
            </div>   
            
        </div>
        <ul v-if="userdetail.authCheckResCode != 'success'" class="navBar">
            <li @click="clickStar('确认通过审核？', 1, 'Authen')"><span class="green-hint">通过</span></li>
            <li @click="clickStar('确认不通过审核？', 0, 'Authen')"><span class="red-hint">不通过</span></li>
        </ul>
    </div>
</template>

<script>
import { MessageBox, Toast } from 'mint-ui';
let moment = require('moment');
export default {
    data () {
        return {
            id: 0,
            userdetail: {},
            paramsList: []
        }
    },
    methods: {
        clickStar(mes, index, key) {
            MessageBox.confirm(mes).then(action => {
                this.submitAudit(index, key)
            },function(){
                console.log('取消了');
            })
        },
        getUserInfo() { // 请求个人信息
            const id =this.id
            this.instance.doctorClinicApplyByIdC({
                id
            })
                .then((response) => {
                    this.userdetail = response.data.result.item
                    this.userdetail[i].createdOn = moment(this.userdetail[i].createdOn).format('YYYY-MM-DD HH:mm:ss')
                })
                .catch((error) => {
                }) 
            
        },
        getParams() { // 请求审核类型
            this.instance.yaeherPatientParameterListD({
                type:'ConfigPar',
                systemCode: 'CheckType', 
            })
                .then((response) => {
                    this.paramsList = response.data.result.item
                })
                .catch((error) => {
                }) 
        },
        submitAudit(index, key) { // 提交审核
            const id = this.id
            const checkRes = this.paramsList[index].code
            this.instance.updateDoctorClinicApplyC({
                id,
                checkRes
            })
                .then((response) => {
                    if (response.data.result.code === 200) {
                        Toast('操作成功')
                        this.$router.go(-1)
                    }
                })
                .catch((error) => {
                }) 
            
        }
    },
    mounted () {
        this.getUserInfo()
        this.getParams()
    },
    created () {
        this.id = parseInt(this.$route.query.id)
        console.log(this.id)
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.auditClinicDetail{
    margin-bottom: 50px;
    .doctorInfo {
        display: flex;
        background-color: $color-wfont;
        padding: 15px;
        .imgCase {
            width: 80px;
            height: 80px;
            img {
                display: block;
                width: 100%;
                height: 100%;
                border-radius: 5px;
            }
        }
        .infoCase {
            flex: 1;
            font-size: $font-xl;
            padding-left: 10px;
            padding-top: 6px;
        }
    }
    .border-case {
        background-color: $color-wfont;
        .auth-case {
            padding: 10px 0 15px;
            .doctor-auth {
                text-align: center;
                flex: 1;
                img {
                    width: 160px;
                    height: 120px;
                }
                p {
                    line-height: 25px;
                }
            }
        }
    }   
}

</style>