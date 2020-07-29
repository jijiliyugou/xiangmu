<template>
    <div class="number-detail padding-top">
        <mt-header fixed title="成员信息">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>        
        </mt-header>
        <div class="content">
            <div class="patient-info add-number-case">
                <mt-field label="姓名" placeholder="请输入咨询人姓名" v-model="leaguerName"></mt-field>
                <mt-field label="手机号" placeholder="请输入手机号" type="tel" v-model="phoneNumber"></mt-field>
                <div class="illness border-top" @click="openPicker">
                    <div class="illness-case">
                        <p class="illness-label">出生年月</p>
                        <p v-if="!birthday" class="illness-value">请选择出生年月</p>
                        <p v-if="birthday" class="illness-value">{{birthday}}</p>
                    </div>
                </div>
                <div class="illness border-bottom" @click="selectIllness(2)">
                    <div class="illness-case">
                        <p class="illness-label">性别</p>
                        <p v-if="!sexName" class="illness-value">请选择性别</p>
                        <p v-if="sexName" class="illness-value">{{sexName}}</p>
                    </div>
                </div>
                <div class="illness" @click="selectIllness(3)">
                    <div class="illness-case">
                        <p class="illness-label">过敏史</p>
                        <p v-if="!allergy" class="illness-value">请选择过敏史</p>
                        <p v-if="allergy" class="illness-value">{{allergy}}</p>
                    </div>
                </div>
                <mt-field v-show="allergy==='有过敏史'" label="输入过敏史" placeholder="请输入过敏史" type="text" v-model="allergicHistory"></mt-field>
            </div>
            <div class="okCase numberOk">
                <a @click="alterLeaguerInfo" class="okBtn">
                    <mt-button type="primary" class="mint-button--large">保存</mt-button>
                </a>
            </div>
            <div class="okCase numberOk onBtn">
                <a @click="deleteNumber" to="/user-number" class="okBtn">
                    <mt-button type="default" class="mint-button--large">删除咨询人</mt-button>
                </a>
            </div>
            
        </div>
        <mt-picker :slots="dataList2" ref="picker2" :showToolbar="true" v-show="show1">
            <div @click="selectIllness(2)" class="slots-no">取消</div>
            <div @click="getPickerValue(2)" class="slots-ok">确认</div>
        </mt-picker>
        <mt-picker :slots="dataList3" ref="picker3" :showToolbar="true" v-show="show2">
            <div @click="selectIllness(3)" class="slots-no">取消</div>
            <div @click="getPickerValue(3)" class="slots-ok">确认</div>
        </mt-picker>
        <mt-datetime-picker
            ref="picker"
            :startDate="startDate"
            :endDate = "endDate"
            v-model="dataTime"
            type="date"
            @confirm="handleConfirm"
            year-format="{value} 年"
            month-format="{value} 月"
            date-format="{value} 日">
        </mt-datetime-picker>
    </div>
</template>

<script>
import { Toast } from 'mint-ui';
import { verifyPrice } from 'assets/js/common.js'
export default {
    data () {
        return {
            id: 0,
            leaguerName: '',
            orderId: 0,
            phoneNumber: '',
            sex: 0,
            sexName: '',
            allergy: '',
            show1: false,
            show2: false,
            hasAllergic: false,
            allergicHistory: '',
            dataTime: '',
            startDate: new Date(0),
            endDate: new Date(),
            birthday: '',
            dataList2: [
                {
                    flex: 1,
                    values: ['男', '女'],
                    className: 'slot2',
                    textAlign: 'center'
                }
            ],
            dataList3: [
                {
                    flex: 1,
                    values: ['无过敏史', '有过敏史'],
                    className: 'slot3',
                    textAlign: 'center'
                }
            ]
        }
    },
    methods: {
        selectIllness(index) { // 呼出咨询人或疾病类型
            if (index === 2) {
                this.show1 = !this.show1
            } else {
                this.show2 = !this.show2
            }
            
        },
        getPickerValue(nub) {
            if (nub === 2) { // 获取性别
                this.show1 = !this.show1
                this.sexName = this.$refs.picker2.getValues()[0]
                const sexArry = ['', '男', '女']
                this.sex = sexArry.indexOf(this.sexName)
            } else { // 获取过敏史
                this.show2 = !this.show2
                this.allergy = this.$refs.picker3.getValues()[0]
                if (this.allergy==='有过敏史') {
                    this.hasAllergic = true
                } else {
                    this.hasAllergic = false
                }
            }   
            
        },
        getnumberDetail() { // 获取成员详情
            const id = this.id
            this.instance.leaguerInfoById({
                id
            })
                .then((response) => {
                    const numberDetail = response.data.result.item
                    this.leaguerName = numberDetail.leaguerName
                    this.sex = numberDetail.sex
                    const sexArry = ['', '男', '女']
                    this.sexName = sexArry[this.sex]
                    this.phoneNumber = numberDetail.phoneNumber
                    this.birthday = numberDetail.birthday.substr(0, 10)
                    let s = this.birthday.replace(/-/g,"/")
                    s = s.replace(/(\.\d+)?/g,"")
                    this.dataTime = new Date(s)
                    console.log(this.dataTime)
                    this.hasAllergic = numberDetail.hasAllergic
                    this.hasAllergic ? this.allergy = '有过敏史' :  this.allergy = '无过敏史'
                    this.allergicHistory = numberDetail.allergicHistory
                })
                .catch((error) => {
                }) 
        },
        deleteNumber() { // 删除成员
            const id = this.id
            this.instance.deleteLeaguerInfo({
                id
            })
                .then((response) => {
                    if (response.data.result.code === 200) {
                        Toast('删除成功')
                        this.$router.push({ 
                            path: '/user-number'
                        })
                    }
                })
                .catch((error) => {
                }) 
        },
        alterLeaguerInfo() { // 修改咨询人
            const id = this.id
            const leaguerName = this.leaguerName
            const phoneNumber = this.phoneNumber
            const hasAllergic = this.hasAllergic
            const birthday = this.birthday
            const allergy = this.allergy
            const allergicHistory  = this.allergicHistory 
            const sex = this.sex
            let verifyJson = [
                {
                    value: leaguerName,
                    msg: '姓名不能为空'
                },
                {
                    value: phoneNumber,
                    msg: '手机号不能为空'
                },
                {
                    value: birthday,
                    msg: '出生年月不能为空'
                },
                {
                    value: sex,
                    msg: '性别不能为空'
                },
                {
                    value: allergy,
                    msg: '过敏史不能为空'
                },
            ]

            let verifyPriceFlg = verifyPrice(verifyJson)
            if (verifyPriceFlg === 1) return
            if (hasAllergic) {
                if (!allergicHistory) {
                    Toast('过敏史不能为空')
                    return
                }
            }
            this.instance.updateLeaguerInfo({
                id,
                leaguerName,
                phoneNumber,
                birthday,
                sex,
                hasAllergic,
                allergicHistory
            })
                .then((response) => {
                    if (response.data.result.code === 200) {
                        Toast('修改成功')
                        this.$router.push({ 
                            path: '/user-number'
                        })
                    }
                    
                })
                .catch((error) => {
                    console.log('科室对应标签')
                }) 
        },
        openPicker() {
            this.$refs.picker.open();
        },
        handleConfirm() {
            let a = this.pickerValue
            this.birthday=this.formatDate(this.$refs.picker.value)
            console.log(this.formatDate(this.$refs.picker.value))
        },
        formatDate(date) {
            const y = date.getFullYear()
            let m = date.getMonth() + 1
            m = m < 10 ? '0' + m : m
            let d = date.getDate()
            d = d < 10 ? ('0' + d) : d
            return y + '-' + m + '-' + d
        }
    },
    mounted () {
        this.getnumberDetail()
    },
    created () {
        this.id = parseInt(this.$route.query.id)
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.number-detail {
    .add-number-case {margin-top: 10px;}
    .answer {display: flex; font-size: $font-l; padding: 10px 15px; margin: 15px 0; background: $color-wfont;}
    .answer > div {flex: 1;}
    .doctorAn { text-align: right;}
    .answer-name {line-height: 20px;}
    .doctorAn img, .doctorAn span {display: inline-block; vertical-align: middle;}
    .picker {position: absolute; bottom: 0; left: 0; right: 0; background: $color-wfont; z-index: 10;}
    .upload-row {height: 145px; position: relative; background: $color-wfont; padding: 15px 10px;}
    .uploadImg{font-size: 50px;padding: 20px;border: 2px dashed #aaa;width: 60px; text-align: center; margin: 10px auto;}
    .okCase {padding: 0 10px 10px 10px; background: $color-wfont;}
    .illness {
            background-color: $color-wfont; 
            padding: 0 0 0 10px;
            .illness-case {
                display: flex; 
                padding: 10px 0;
                .illness-label {
                    width: 105px;
                }
            }
        }
    .numberOk {padding-top: 15px;}
    .onBtn {padding: 10px 10px 30px;}
    .mint-cell-title {display: block;}
}

</style>