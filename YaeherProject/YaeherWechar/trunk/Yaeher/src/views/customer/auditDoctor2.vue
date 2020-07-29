<template>
    <div class="audit-doctor2 padding-top">
        <mt-header fixed title="医生认证审核">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
            <mt-button slot="right">
                <router-link :to="{path: '/doctor-detail-patient', query:{id}}" class="right-white">查看详情</router-link>
            </mt-button> 
        </mt-header>
        <div class="content p-content">    
            <!-- <form> -->
                <div class="patient-info">
                    <mt-field label="真实姓名:" placeholder="" v-model="userdetail.doctorName" readonly></mt-field>
                    <mt-field label="认证类型:" placeholder="" v-model="userdetail.authType" readonly></mt-field>
                    <mt-field label="性别" placeholder="" v-model="userdetail.sex" readonly></mt-field>
                    <mt-field label="所在医院:" placeholder="" v-model="userdetail.hospitalName" readonly></mt-field>
                    <mt-field label="所在科室:" placeholder="" v-model="userdetail.department" readonly></mt-field>
                    <mt-field label="职称:" placeholder="" v-model="userdetail.title" readonly></mt-field>
                </div>
                <div> 
                    <div class="border-case">
                        <div class="flex auth-case">
                            <div class="doctor-auth">
                                <p>身份证正面照</p>
                                <img v-gallery  :src="idcardup"/>
                            </div>
                            <div class="doctor-auth">
                                <p>身份证背面照</p>
                                <img v-gallery  :src="idcarddown" />
                            </div>
                        </div>
                    </div>
                    <div class="border-top border-case">    
                        <div class="flex auth-case">
                            <div class="doctor-auth">
                                <p>资格证</p>
                                <img v-gallery  :src="qualificationcertificate" />
                            </div>
                            <div class="doctor-auth">
                                <p>执业证</p>
                                <img v-gallery  :src="certificateofpractice"  />
                            </div>
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

export default {
    data () {
        return {
            id: 0,
            userdetail: {},
            file: [],
            paramsList: [],
            idcarddown: '',
            idcardup: '',
            certificateofpractice: '',
            qualificationcertificate: ''
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
            this.instance.customerServiceYaeherDoctorByIdC({
                id
            })
                .then((response) => {
                    this.userdetail = response.data.result.item
                    this.userdetail.sex = ['', '男', '女'][this.userdetail.sex]
                    let file = this.userdetail.file
                    for(let i = 0; i < file.length; i++) {
                        if(file[i].typeDetail === 'idcarddown') this.idcarddown = file[i].address
                        if(file[i].typeDetail === 'idcardup') this.idcardup = file[i].address
                        if(file[i].typeDetail === 'certificateofpractice') this.certificateofpractice = file[i].address
                        if(file[i].typeDetail === 'qualificationcertificate') this.qualificationcertificate = file[i].address
                    }
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
            const authCheckRes = this.paramsList[index].code
            this.instance.authCheckYaeherDoctorA({
                id,
                authCheckRes,
                AuthCheck: key
            })
                .then((response) => {
                    if (response.data.result.code === 200) {
                        Toast('操作成功')
                        this.$router.push({ 
                            path: '/index-customer'
                        })
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


.audit-doctor2 {
    margin-bottom: 50px;
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