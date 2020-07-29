<template>
    <div class="add-arrange padding-top">
        <mt-header fixed :title="title">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
            <div class="arrange-case">
                <div class="illness" @click="openPicker">
                    <div class="illness-case">
                        <p class="illness-label">时间</p>
                        <p v-if="!schedulingDate" class="illness-value">请选择时间</p>
                        <p v-if="schedulingDate" class="illness-value">{{schedulingDate}}</p>
                    </div>
                </div>

                <mt-checklist
                title="时段"
                v-model="value"
                :options="options">
                </mt-checklist>

                <mt-radio
                title="重复方式"
                v-model="value1"
                :options="options1">
                </mt-radio>

                <mt-radio
                title="门诊类型"
                v-model="value2"
                :options="options2">
                </mt-radio>

                <mt-radio
                class="openClose"
                title="是否开启"
                v-model="value3"
                :options="options3">
                </mt-radio>
                
                <p class="line-p"></p>
                <mt-field label="门诊地点" placeholder="请输入门诊地点" v-model="clinicIDAdd"></mt-field>
                <mt-field label="挂号费" type="number" placeholder="请输入挂号费" v-model="registrationFee"></mt-field>
                
                <div v-if="id===0" class="set-ok">
                    <mt-button @click="createlable" type="primary" class="mint-button--large">保存</mt-button>
                </div>
                <div v-if="id!=0" class="set-ok">
                    <mt-button @click="alterlable" type="primary" class="mint-button--large">保存</mt-button>
                </div>
                <div v-if="id!=0" class="set-ok">
                    <mt-button @click="deleteLable" type="default" class="mint-button--large">删除</mt-button>
                </div>
            </div>
            
        </div>
        <mt-datetime-picker
            ref="picker"
            v-model="pickerValue"
            type="date"
            :startDate="startDate"
            @confirm="handleConfirm"
            year-format="{value} 年"
            month-format="{value} 月"
            date-format="{value} 日">
        </mt-datetime-picker>
    </div>
</template>

<script>
import { Toast, MessageBox } from 'mint-ui';
import { verifyPrice } from 'assets/js/common.js'
export default {
    data () {
        return {
            id: 0,
            clinicId: 0,
            title: '',
            show: false,
            registrationFee: '',
            clinicIDAdd: '',
            value: ['1'],
            value1: '1',
            value2: '1',
            pickerValue: '',
            startDate: new Date(),
            schedulingDate: '',
            value3:'1',
            options3: [
                {   
                    label: '启用',
                    value: '1'
                },
                {
                    label: '关闭',
                    value: '0'
                }
            ],
            paramsList: [],
            clinicTypeList: [],
            duplicationList: [],
            schedulingTimeList: [],
            clicicArry: [],
            duplicationArry: [],
            schedulingArry: []
        }
    },
    methods: {
        openPicker() {
            this.$refs.picker.open();
        },
        handleConfirm() {
            this.schedulingDate=this.formatDate(this.$refs.picker.value)
            this.show = true
        },
        formatDate(date) {
            const y = date.getFullYear()
            let m = date.getMonth() + 1
            m = m < 10 ? '0' + m : m
            let d = date.getDate()
            d = d < 10 ? ('0' + d) : d
            return y + '-' + m + '-' + d
        },
        getparams() { // 获取参数
            this.instance.doctorSchedulingTypeD({
            })
                .then((response) => {
                    this.paramsList = response.data.result.item
                    this.clinicTypeList = this.paramsList.clinicType
                    this.duplicationList = this.paramsList.duplication
                    this.schedulingTimeList = this.paramsList.schedulingTime
                    this.clicicArry = this.arrOption(this.clinicTypeList) // 门诊类型
                    this.value2 = this.clicicArry[0].value
                    this.duplicationArry = this.arrOption(this.duplicationList) // 重复方式
                    this.value1 = this.duplicationArry[0].value
                    this.schedulingArry = this.arrOption(this.schedulingTimeList)  // 时间
                    this.value = [this.schedulingArry[0].value]
                    if (this.id != 0) {
                        this.serviceDetail()
                    }
                    
                })
                .catch((error) => {
                }) 
        },
        serviceDetail() { // 获取门诊排班详情
            const id = this.id
            this.instance.doctorSchedulingByIDD({
                id
            })
                .then((response) => {
                    let serveretail = response.data.result.item
                    this.clinicIDAdd = serveretail.clinicIDAdd
                    this.registrationFee = serveretail.registrationFee
                    this.schedulingDate = serveretail.schedulingDateUtc
                    let newTime = new Date(0)
                    this.pickerValue = newTime
                    console.log(this.pickerValue)
                    const duplication = JSON.parse(serveretail.duplication)
                    this.value1 = duplication.Code
                    const clinicType = JSON.parse(serveretail.clinicType)
                    this.value2 = clinicType.Code
                    this.value = []
                    let serviceState = serveretail.serviceState
                    if (serviceState) {
                        this.value3 = '1'
                    } else {
                        this.value3 = '0'
                    }
                    const schedulingTime = JSON.parse(serveretail.schedulingTime)
                    for (let i=0;i<schedulingTime.length;i++) {
                        this.value.push(schedulingTime[i].Code)
                    }
                    this.clinicId = serveretail.id
                    console.log(this.clinicId)
                })
                .catch((error) => {
                }) 
        },
        arrOption(list) { // 数组数据处理
            let clicicArry = new Array()
            for (let i = 0; i< list.length; i++) {
                let obj = {
                    label: list[i].value,
                    value: list[i].code
                }
                clicicArry.push(obj)
            }
            return clicicArry
        },
        createlable() { // 添加门诊排班
            const schedulingDate = this.schedulingDate
            let schedulingTimeList = []
            for (let i = 0; i < this.value.length; i++) {
                let obj = {
                    code: this.value[i]
                }
                schedulingTimeList.push(obj)
            }
            console.log(schedulingTimeList)
            const duplication = this.value1
            const clinicType = this.value2
            const clinicIDAdd = this.clinicIDAdd
            let registrationFee = this.registrationFee
            let serviceState = parseInt(this.value3)

            // 校验数组
            let verifyJson = [
                {
                    value: clinicIDAdd,
                    msg: '门诊地点不能为空'
                },
                {
                    value: registrationFee,
                    msg: '挂号费不能为空'
                },
            ]
            registrationFee = parseFloat(registrationFee)
            let verifyPriceFlg = verifyPrice(verifyJson)
            if (verifyPriceFlg === 1) return

            this.instance.createDoctorSchedulingD({
                schedulingDate,
                schedulingTimeList,
                duplication,
                clinicType,
                clinicIDAdd,
                registrationFee,
                serviceState
            })
                .then((response) => {
                    if (response.data.result.code === 200) {
                        Toast('添加成功')
                        this.$router.push({ 
                            path: '/arrange'
                        })
                    }
                    
                })
                .catch((error) => {
                }) 
        },
        alterlable() { // 修改门诊排班
            const schedulingDate = this.schedulingDate
            let schedulingTimeList = []
            for (let i = 0; i < this.value.length; i++) {
                let obj = {
                    code: this.value[i]
                }
                schedulingTimeList.push(obj)
            }
            const duplication = this.value1
            const clinicType = this.value2
            const clinicIDAdd = this.clinicIDAdd
            let registrationFee = this.registrationFee
            const serviceState = parseInt(this.value3)
            const id = this.clinicId
            // 校验数组
            let verifyJson = [
                {
                    value: clinicIDAdd,
                    msg: '门诊地点不能为空'
                },
                {
                    value: registrationFee,
                    msg: '挂号费不能为空'
                },
            ]
            registrationFee = parseFloat(registrationFee)
            let verifyPriceFlg = verifyPrice(verifyJson)
            if (verifyPriceFlg === 1) return

            this.instance.updateDoctorSchedulingD({
                id,
                schedulingDate,
                schedulingTimeList,
                duplication,
                clinicType,
                clinicIDAdd,
                registrationFee,
                serviceState
            })
                .then((response) => {
                    if (response.data.result.code === 200) {
                        Toast('修改成功')
                        this.$router.push({ 
                            path: '/arrange'
                        })
                    }
                    
                })
                .catch((error) => {
                }) 
        },
        deleteLable() {
            let id = this.id
            MessageBox.confirm('是否删除该门诊排班').then(action => {
                this.instance.deleteDoctorSchedulingD({
                    id
                })
                    .then((response) => {
                        if (response.data.result.code === 200) {
                            Toast('删除成功')
                            this.$router.push({ 
                                path: '/arrange'
                            })
                        }
                        
                    })
                    .catch((error) => {
                    }) 
            },function(){
                console.log('取消了');
            })
            
        }
    },
    computed: {
        options2() {
            let dataSlots = this.clicicArry
            return dataSlots
        },
        options1() {
            let dataSlots1 = this.duplicationArry
            return dataSlots1
        },
        options() {
            let dataSlots2 = this.schedulingArry
            return dataSlots2
        }
    },    
    mounted () {
        this.getparams()
    },
    created () {
        let id = this.$route.query.id
        if(id) {
            this.title = '门诊排班详情'
            this.id = parseInt(id)
        } else {
            this.id = 0
            this.title = '添加门诊排班'
            let newTime = new Date()
            this.pickerValue = newTime
            let timeDate = this.formatDate(newTime)
            this.schedulingDate = timeDate
        }
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.add-arrange {
    .arrange-case{
        .mint-checklist {
            .mint-cell {
                display: inline-block;
                width: 33.3333%;
                background-image: none;
                .mint-cell-wrapper {
                    background-image: none;
                    .mint-checkbox-core::after {
                        top: 4px;
                        left: 7px;
                    }
                }
            }
        }
        .mint-radiolist {
            .mint-cell {
                display: inline-block;
                width: 33.3333%;
                background-image: none;
                .mint-cell-wrapper {
                    background-image: none;
                }
            }
            
        }
        .openClose {
            .mint-cell {
                display: inline-block;
                width: 50%;
                background-image: none;
                .mint-cell-wrapper {
                    background-image: none;
                }
            }
        }
        .line-p {
            height: 5px;
        }
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
        .set-ok {
            margin: 20px 10px;
        }
    }
}





</style>