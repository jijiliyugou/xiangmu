<template>
    <div class="add-number padding-top">
        <mt-header fixed title="添加成员">
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
                        <p v-if="!birthday" class="illness-value plValue">请选择出生年月</p>
                        <p v-if="birthday" class="illness-value">{{birthday}}</p>
                    </div>
                </div>
                
                <div class="illness border-top" @click="selectIllness(2)">
                    <div class="illness-case">
                        <p class="illness-label">性别</p>
                        <p v-if="!sexName" class="illness-value plValue">请选择性别</p>
                        <p v-if="sexName" class="illness-value">{{sexName}}</p>
                    </div>
                </div>
                <div class="illness border-top" @click="selectIllness(3)">
                    <div class="illness-case">
                        <p class="illness-label">过敏史</p>
                        <p v-if="!allergy" class="illness-value plValue">请选择过敏史</p>
                        <p v-if="allergy" class="illness-value">{{allergy}}</p>
                    </div>
                </div>
                <mt-field v-show="allergy==='有过敏史'" label="过敏药品或过敏物" placeholder="请输入过敏药品或过敏物" type="text" v-model="allergicHistory"></mt-field>
            </div>
            <div v-if="!orderId" class="okCase numberOk">
                <a @click="createLeaguerInfo" class="okBtn">
                    <mt-button type="primary" class="mint-button--large">保存</mt-button>
                </a>
            </div>
            <div v-if="orderId" class="okCase numberOk">
                <a  @click="createLeaguerInfo" class="okBtn">
                    <mt-button type="primary" class="mint-button--large">下一步</mt-button>
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
import { verifyPrice, formatDate } from 'assets/js/common.js'
import { Toast } from 'mint-ui';
export default {
    data () {
        return {
            leaguerName: '',
            orderId: 0,
            phoneNumber: '',
            sex: 0,
            sexName: '',
            allergy: '',
            show: false,
            show1: false,
            show2: false,
            hasAllergic: false,
            price: 0,
            allergicHistory: '',
            dataTime: '',
            startDate: new Date('1900-01-01'),
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
        createLeaguerInfo() { // 添加咨询人
            const orderId = this.orderId
            const price = this.price
            const serviceType = this.serviceType
            const leaguerName = this.leaguerName
            const phoneNumber = this.phoneNumber
            const hasAllergic = this.hasAllergic
            const allergy = this.allergy
            const birthday = this.birthday
            const sex = this.sex
            const allergicHistory = this.allergicHistory
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
            if (leaguerName.length > 20) {
                Toast('姓名不能超过20字')
                return
            }
            if (hasAllergic) {
                if (!allergicHistory) {
                    Toast('过敏史不能为空')
                    return
                }
            }
            console.log('认证成功')
            this.instance.createLeaguerInfo({
                leaguerName,
                phoneNumber,
                birthday,
                sex,
                hasAllergic,
                allergicHistory
            })
                .then((response) => {
                    if (response.data.result.code === 200) {
                        Toast('添加成功')
                        if (orderId) {
                            this.$router.push({ 
                                path: '/consultation',
                                query: {
                                    id: orderId,
                                    add: 'new',
                                    price,
                                    serviceType
                                }
                            })
                        } else {
                            this.$router.push({ 
                                path: '/user-number'
                            })
                        }
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
            this.birthday=this.formatDate(this.$refs.picker.value)
            this.show = true
        },
        defaultTime() { // 设置默认时间
            let newTime = new Date()
            this.dataTime = newTime
            let timeDate = formatDate(newTime, 0)
            // this.birthday = timeDate.dataTime
        },
        formatDate(date) {
            let y = date.getFullYear()
            let m = date.getMonth() + 1
            m = m < 10 ? '0' + m : m
            let d = date.getDate()
            d = d < 10 ? ('0' + d) : d
            return y + '-' + m + '-' + d
        }
    },
    created () {
        this.orderId = parseInt(this.$route.query.id)
        this.price = this.$route.query.price
        this.serviceType = this.$route.query.serviceType
        this.defaultTime()
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.add-number {
    .add-number-case {margin-top: 10px;}
    .answer {display: flex; font-size: $font-l; padding: 10px 15px; margin: 15px 0; background: $color-wfont;}
    .answer > div {flex: 1;}
    .doctorAn { text-align: right;}
    .answer-name {line-height: 20px;}
    .doctorAn img, .doctorAn span {display: inline-block; vertical-align: middle;}
    .picker {position: absolute; bottom: 0; left: 0; right: 0; background: $color-wfont; z-index: 10;}
    .upload-row {height: 145px; position: relative; background: $color-wfont; padding: 15px 10px;}
    .uploadImg{font-size: 50px;padding: 20px;border: 2px dashed #aaa;width: 60px; text-align: center; margin: 10px auto;}
    .okCase {padding: 0 10px 20px 10px; background: $color-wfont;}
    .illness {background-color: $color-wfont; padding: 0 0 0 10px;}
    .illness-case {display: flex; padding: 10px 0;}
    .illness-label {width: 105px;}
    .illness-label::after {content: '*'; color: $color-red;vertical-align: bottom; line-height: 18px; padding-left: 1px;}
    .mint-cell-text::after {content: '*'; color: $color-red;vertical-align: bottom; line-height: 10px; padding-left: 1px;}
    .numberOk {padding-top: 15px;}
}

</style>