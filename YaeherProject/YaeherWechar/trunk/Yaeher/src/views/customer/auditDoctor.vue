<template>
    <div class="audit-doctor padding-top">
        <mt-header fixed title="医生审核">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
            <mt-button v-if="type != 'admin' && baseTestRes === 'success'"  slot="right">
                <router-link  :to="{path: '/audit-doctor2', query: {id}}" class="right-white">查看认证</router-link>
            </mt-button> 
        </mt-header>
        <div class="content">    
            <div class="patient-info">
                <mt-field label="姓名:" placeholder="" v-model="doctorName" readonly></mt-field>
                <mt-field label="工作医院:" placeholder="" v-model="hospitalName" readonly></mt-field>
                <mt-field label="所在专科:" placeholder="" v-model="department" readonly></mt-field>
                <mt-field label="工作年限:" placeholder="" v-model="workYear" readonly></mt-field>
                <mt-field label="职称:" placeholder="" v-model="title" readonly></mt-field>
                <mt-field label="毕业学校:" placeholder="" v-model="graduateSchool" readonly></mt-field>
                <mt-field label="是否相信中医:" placeholder="" v-model="isBelieveTCM" readonly></mt-field>
                <mt-field label="是否觉得自己有服务意识:" placeholder="" v-model="isServiceConscious" readonly></mt-field>
                <mt-field label="您的微信号:" placeholder="" v-model="wechatNum" readonly></mt-field>
                <mt-field label="手机号:" placeholder="" type="tel" v-model="phoneNumber" readonly></mt-field>
                <mt-field label="推荐人:" placeholder="" v-model="recommenderName" readonly></mt-field>
                <!-- <div v-if="type != 'admin' && baseTestRes === 'success'" class="doctorTest">
                    <p>
                        医生考试状态
                    </p>
                    <p v-if="baseTestRes === 'success'" class="green-hint">通过</p>
                    <p v-if="baseTestRes != 'success'"  class="red-hint">不通过</p>
                </div> -->
                <div v-if="type != 'admin' && baseTestRes != 'success'" class="doctorTest">
                    <p>
                        医生考试是否通过
                    </p>
                    <div class="testCase">
                        <mt-button @click="clickStar('医生考试通过？', 1, 'Test')" type="primary">通过</mt-button>
                        <mt-button class="noBtn" @click="clickStar('医生考试不通过？', 0, 'Test')" type="danger">不通过</mt-button>
                    </div>
                </div>
            </div>
            
        </div>
        <ul v-if="checkRes === 'checking'" class="navBar">
            <li @click="clickStar('确认通过审核？', 1, 'Check')"><span class="green-hint">通过</span></li>
            <li @click="clickStar('确认不通过审核？', 0, 'Check')"><span class="red-hint">不通过</span></li>
        </ul>
    </div>
</template>

<script>
import { MessageBox, Toast } from 'mint-ui';

export default {
    data () {
        return {
            id: 0,
            doctorName: '',
            hospitalName: '',
            phoneNumber: '',
            department: '',
            workYear: '',
            title: '',
            graduateSchool: '',
            isBelieveTCM: '',
            isServiceConscious: '',
            wechatNum: '',
            recommenderName: '',
            successTest: '',
            type: '',
            show: false,
            checkRes: '',
            baseTestRes: false,
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
            const type = this.type
            if (type != 'admin') {
                this.instance.customerServiceYaeherDoctorByIdC({
                    id
                })
                    .then((response) => {
                        let userdetail = response.data.result.item
                        this.doctorName = userdetail.doctorName
                        this.hospitalName = userdetail.hospitalName
                        this.phoneNumber = userdetail.phoneNumber
                        this.department = userdetail.department
                        this.workYear = userdetail.workYear
                        this.title = userdetail.title
                        this.graduateSchool = userdetail.graduateSchool
                        this.isBelieveTCM = userdetail.isBelieveTCM
                        this.isBelieveTCM ? this.isBelieveTCM = '是' : this.isBelieveTCM ='否'
                        this.isServiceConscious = userdetail.isServiceConscious
                        this.isServiceConscious ? this.isServiceConscious = '是' : this.isServiceConscious ='否'
                        this.wechatNum = userdetail.wechatNum
                        this.checkRes = userdetail.checkRes
                        this.baseTestRes = userdetail.baseTestRes
                        this.recommenderName = userdetail.recommenderName
                        
                        
                    })
                    .catch((error) => {
                    }) 
            } else {
                this.instance.yaeherDoctorById({
                    id
                })
                    .then((response) => {
                        let userdetail = response.data.result.item
                        this.doctorName = userdetail.doctorName
                        this.hospitalName = userdetail.hospitalName
                        this.phoneNumber = userdetail.phoneNumber
                        this.department = userdetail.department
                        this.workYear = userdetail.workYear
                        this.title = userdetail.title
                        this.graduateSchool = userdetail.graduateSchool
                        this.isBelieveTCM = userdetail.isBelieveTCM
                        this.isBelieveTCM ? this.isBelieveTCM = '是' : this.isBelieveTCM ='否'
                        this.isServiceConscious = userdetail.isServiceConscious
                        this.isServiceConscious ? this.isServiceConscious = '是' : this.isServiceConscious ='否'
                        this.wechatNum = userdetail.wechatNum
                        this.recommenderName = userdetail.recommenderName
                        this.checkRes = userdetail.checkRes
                        this.baseTestRes = userdetail.baseTestRes
                    })
                    .catch((error) => {
                    }) 
            }
            
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
            if(key === 'Test') {
                this.instance.authCheckYaeherDoctorA({
                    id,
                    checkRes,
                    AuthCheck: key,
                    BaseTestRes: checkRes,
                    SimTestRes: checkRes
                })
                    .then((response) => {
                        if (response.data.result.code === 200) {
                            Toast('操作成功')
                            this.$router.go(-1)
                        }
                    })
                    .catch((error) => {
                    })     
            } else {
                this.instance.authCheckYaeherDoctorA({
                    id,
                    checkRes,
                    AuthCheck: key
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
            
        }
    },
    mounted () {
        this.getUserInfo()
        this.getParams()
    },
    created () {
        this.type = this.$route.query.type
        this.id = parseInt(this.$route.query.id)
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";


.audit-doctor {
    margin-bottom: 50px;
    .doctorTest {
        background: #fff;
        margin-top: 10px;
        padding: 10px 20px 20px;;
        p {
            text-align: center;
            font-size: 18px;
            line-height: 48px;
        }
        .testCase {
            button {
                width: 100px;
                height: 38px;
            }
            .noBtn {
                float: right;
            }
        }
    }
}
</style>