<template>
    <div class="authentication1 padding-top">
        <mt-header fixed title="怡禾认证1/3">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
            <mt-button  slot="right">
                <a @click="authTypeSelect" class="right-white">下一步</a>
            </mt-button> 
        </mt-header>
        <div class="content">
            <div class="patient-info add-number-case">
                <div class="illness" @click="selectSex(true)">
                    <div class="illness-case border-nome">
                        <p class="illness-label">认证类型</p>
                        <p v-if="type===''" class="illness-value">请选择认证类型</p>
                        <p v-if="type!=''" class="illness-value">{{type}}</p>
                    </div>                   
                </div>
                <mt-field label="真实姓名" placeholder="" readonly v-model="doctorName"></mt-field>
                <div class="illness border-top">
                    <div class="illness-case">
                        <p class="illness-label">性别</p>
                        <p v-if="sex1===''" class="illness-value">请选择性别</p>
                        <p v-if="sex1!=''" class="illness-value">{{sex1}}</p>
                    </div>
                </div>
                <mt-field label="所在医院" readonly placeholder="请输入所在医院" v-model="hospitalName"></mt-field>
                <mt-field label="所在科室" readonly placeholder="请输入所在科室" v-model="department"></mt-field>
                <mt-field label="职称" readonly placeholder="请输入职称" v-model="title"></mt-field>
            </div>
            <!-- <div class="okCase">
                <mt-button @click="submitauthentication" type="primary" class="mint-button--large">保存</mt-button>
            </div> -->
            
            <!-- <mt-picker :slots="slots" ref="picker1" :showToolbar="true" v-show="show1">
                <div @click="selectSex(false)" class="slots-no">取消</div>
                <div @click="getPickerValue(false)" class="slots-ok">确认</div>
            </mt-picker> -->
            
        </div>
        <mt-picker :slots="slots1" ref="picker" :showToolbar="true" v-show="show">
            <div @click="selectSex(true)" class="slots-no">取消</div>
            <div @click="getPickerValue(true)" class="slots-ok">确认</div>
        </mt-picker>
    </div>
</template>

<script>
import { Toast } from 'mint-ui';
export default {
    data () {
        return {
            doctorName: '',
            hospitalName: '',
            sex: '',
            department: '',
            checkRes: false,
            title: '',
            type: '',
            show: false,
            show1: false,
            sex1: '',
            authType: '',
            slotsArry: [],
            typeList: [],
            slots1: [
                {
                    flex: 1,
                    values: ['临床医师', '药师', '其他医务工作者'],
                    className: 'slot1',
                    textAlign: 'center'
                }
            ]
        }
    },
    methods: {
        getPickerValue(flag) {                      
            if (flag) {
                this.show = !this.show
                this.type = this.$refs.picker.getValues()[0]
                for (let i = 0 ;i < this.typeList.length; i++) {
                    if(this.typeList[i].value === this.type) {
                        this.authType = this.typeList[i].code
                        console.log(this.authType)
                        return
                    } else {
                        this.authType = ''
                    }
                }
            } else {
                this.show1 = !this.show1
                this.sex1 = this.$refs.picker1.getValues()[0]
                const sexArry = ['', '男', '女']
                this.sex = sexArry.indexOf(this.sex1)
            }
        },
        selectSex(flag) {
            if (flag) {
                this.show = !this.show
            } else {
                this.show1 = !this.show1
            }
            
        },
        getUserInfo() { // 请求个人信息
            this.instance.yaeherDoctorD({
            })
                .then((response) => {
                    const userdetail = response.data.result.item
                    this.doctorName = userdetail.doctorName
                    this.hospitalName = userdetail.hospitalName
                    this.department = userdetail.department
                    this.title = userdetail.title
                    this.checkRes = userdetail.checkRes
                    this.sex = userdetail.sex
                    const sexStatus = ['', '男', '女']
                    this.sex1 = sexStatus[this.sex]
                    this.authType = userdetail.authType

                    console.log(this.authType)
                    for (let i = 0 ;i < this.typeList.length; i++) {
                        if(this.typeList[i].code=== this.authType) {
                            this.type = this.typeList[i].value
                            console.log(this.type)
                            return
                        } else {
                            this.type = ''
                        }
                    }
                    const userId = response.data.result.item.id
                    this.id = userId
                    window.sessionStorage.setItem('userId', userId)
                })
                .catch((error) => {
                }) 
        },
        doctorAuthTypeD() { // 请求认证类型
            this.instance.yaeherDoctorAuthTypeD({
            })
                .then((response) => {
                    this.typeList = response.data.result.item
                    for(var j = 0;j < this.typeList.length;j++ ) {
                        this.slotsArry.push(this.typeList[j].value)
                    }
                    this.getUserInfo()

                })
                .catch((error) => {
                }) 
        },
        authTypeSelect () { // 提交认证类型
            const authType = this.authType
            const id = this.id
            if (!authType) {
                Toast('认证类型不能为空')
                return
            }
            this.instance.updateYaeherDoctorD({
                id,
                authType
            })
                .then((response) => {
                    this.$router.push({ 
                        path: '/authentication2',
                        query: {
                            seachValue: this.seachValue,
                            searchType: this.searchType
                        }
                    })
                    
                })
                .catch((error) => {
                }) 
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
        }
    },
    mounted () {
        this.doctorAuthTypeD()
    },
    created () {
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.authentication1 {
    .add-number-case {margin-top: 10px;}
    .answer {display: flex; font-size: $font-l; padding: 10px 15px; margin: 15px 0; background: $color-wfont;}
    .answer > div {flex: 1;}
    .doctorAn { text-align: right;}
    .answer-name {line-height: 20px;}
    .doctorAn img, .doctorAn span {display: inline-block; vertical-align: middle;}
    .picker {position: absolute; bottom: 0; left: 0; right: 0; background: $color-wfont; z-index: 10;}
    .upload-row {height: 145px; position: relative; background: $color-wfont; padding: 15px 10px;}
    .uploadImg{font-size: 50px;padding: 20px;border: 2px dashed #aaa;width: 60px; text-align: center; margin: 10px auto;}
    .okCase {padding: 10px 10px 20px 10px; background: $color-wfont;}
    .illness {background-color: $color-wfont; padding: 0 0 0 10px;}
    .illness-case {display: flex; padding: 10px 0;}
    .illness-label {width: 105px;}
}

</style>