<template>
    <div class="income-statistics padding-top">
        <mt-header fixed title="收入统计">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
            <div class="income-top">
                <div class="illness" >
                    <div class="illness-case">
                        <p @click="selectSex" class="illness-value">{{searchType}}</p>
                        <p @click="openPicker" v-if="!dataTime" class="illness-value">请选择时间</p>
                        <p @click="openPicker" v-if="dataTime" class="illness-value">{{dataTime}}</p>
                    </div>
                </div>
            </div>
            <div>
                <ul>
                    <li class="flexList">
                        收入流水：<span class="green-hint">{{icomeDetail.incomeWater}}</span>
                    </li>
                    <li class="flexList">
                        公司进账：<span class="green-hint">{{icomeDetail.incomeTotal}}</span>
                    </li>
                    <li class="flexList">
                        订单数：<span class="green-hint">{{icomeDetail.orderTotal}}</span>
                    </li>
                    <li class="flexList">
                        订单收入：<span class="green-hint">{{icomeDetail.orderTotalMoney}}</span>
                    </li>
                    <li class="flexList">
                        退单总金额：<span class="red-hint">{{icomeDetail.refundTotalMoney}}</span>
                    </li>

                    <li v-if="yearshow1 !=0" class="flexList">
                        <div>时间</div>
                        <div>收入流水</div>
                        <div>公司进账</div>
                    </li>
                    <router-link v-for="(item, index) in icomeList" :key="index" tag="li" :to="{path: '/time-mouth', query: {timeValue: item.createdOn}}" v-if="yearshow1 ===2"  class="flexList">
                        <div>{{item.createdOn1}}</div>
                        <div>{{item.incomeWater}}</div>
                        <div>
                            {{item.incomeTotal}}
                            <i class="mint-cell-allow-right"></i>
                        </div>
                    </router-link>

                    <router-link v-for="(item, index) in icomeList" :key="index" tag="li" :to="{path: '/time-day', query: {timeValue: item.createdOn}}" v-if="yearshow1 ===1" class="flexList">
                        <div>{{item.createdOn1}}</div>
                        <div>{{item.incomeWater}}</div>
                        <div>
                            {{item.incomeTotal}}
                            <i class="mint-cell-allow-right"></i>
                        </div>
                    </router-link>

                    <router-link v-for="(item, index) in icomeList" :key="index" tag="li" :to="{path: '/order-look', query: {id: item.consultID}}" v-if="yearshow1 ===0"  class="flexList income-case">
                        <div class="flex-left">
                            <p class="icome-name"> 
                                <span v-if="item.consultType === 'ImageText'" class="hint">文</span>
                                <span v-if="item.consultType === 'Phone'"  class="hint">电</span>
                                <span>{{item.doctorName}}</span> 
                                <span class="smallFont">患者：{{item.patientName}}</span>
                            </p>
                            <p class="income-time">{{item.createdOn1}}</p>
                        </div>
                        <div class="flex-right">
                            <p v-if="item.orderMoney>=0" class="green-hint">收入流水：{{item.orderMoney}}</p>
                            <p v-if="item.orderMoney>=0" class="income-time green-hint">公司进账：{{item.proportionMoney}}</p>
                            <p v-if="item.orderMoney<0" class="red-hint">收入流水：{{item.orderMoney}}</p>
                            <p v-if="item.orderMoney<0" class="income-time red-hint">公司进账：{{item.proportionMoney}}</p>
                        </div>
                    </router-link>
                </ul>
            </div>

            
        </div>
        <mt-picker :slots="slots" ref="picker1" :showToolbar="true" v-show="show1">
            <div @click="selectSex()" class="slots-no">取消</div>
            <div @click="getPickerValue()" class="slots-ok">确认</div>
        </mt-picker>

        <mt-datetime-picker
            :class="{'yaerAndMonth': yearshow===1, 'yaerShow': yearshow===2}"
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
import { formatDate } from 'assets/js/common.js'
let moment = require('moment');
export default {
    data () {
        return {
            show: false,
            show1: false,
            yearshow: 1,
            yearshow1: 1,
            timeValue: '',
            icomeDetail: {},
            icomeList: [],
            searchType: '按月搜索',
            price: '',
            address: '',
            value: ['1'],
            value1: '1',
            value2: '1',
            pickerValue: '',
            startDate: new Date(2016, 0, 1),
            dataTime: '请选择时间',
            slots: [
                {
                    flex: 1,
                    values: ['按日搜索', '按月搜索', '按年搜索'],
                    className: 'slot1',
                    textAlign: 'center'
                }
            ]
        }
    },
    methods: {
        openPicker() {
            this.$refs.picker.open();
        },
        handleConfirm() {
            
            this.timeValue = this.$refs.picker.value
            let timeObj = formatDate(this.timeValue , this.yearshow)
            this.dataTime=timeObj.dataTime
            this.startTime = timeObj.startTime
            this.endTime = timeObj.endTime
            this.show = true
            this.yearshow1 = this.yearshow
            this.getAllEvaluate()
            this.getEvaluateList()
        },
        getPickerValue() { 
            const selectArry = ['按日搜索', '按月搜索', '按年搜索'];    
            this.show1 = !this.show1
            this.searchType = this.$refs.picker1.getValues()[0]            
            this.yearshow = selectArry.indexOf(this.searchType)
            
            
            
        },
        selectSex() {
            this.show1 = !this.show1
        },
        getAllEvaluate() { // 获取收入统计
            const incomeType = ['day', 'month', 'year'][this.yearshow]
            const startTime = this.startTime
            const endTime = this.endTime
            this.instance.adminIncomeReportC({
                incomeType,
                startTime,
                endTime
            })
                .then((response) => {
                    let icomeDetail = response.data.result.item
                    if (icomeDetail) {
                        this.icomeDetail = icomeDetail
                    } else {
                        this.show = false
                        Toast('暂无数据')
                    }
                    
                })
                .catch((error) => {
                }) 
        },
        getEvaluateList() { // 获取收入明细
            const startTime = this.startTime
            const endTime = this.endTime
            const incomeType = ['day', 'month', 'year'][this.yearshow]
            this.instance.adminIncomeDetailReportC({
                startTime,
                endTime,
                incomeType,
            })
                .then((response) => {
                    this.icomeList = response.data.result.item
                    if(incomeType === 'day') {
                        for (let i = 0; i < this.icomeList.length; i++) {
                            const createdOn = this.icomeList[i].createdOn
                            this.icomeList[i].createdOn1 = moment(createdOn).format('HH:mm:ss')
                        }
                    }
                    if(incomeType === 'month') {
                        for (let i = 0; i < this.icomeList.length; i++) {
                            const createdOn = this.icomeList[i].createdOn
                            this.icomeList[i].createdOn1 = moment(createdOn).format('MM-DD')
                        }
                    }
                    if(incomeType === 'year') {
                        for (let i = 0; i < this.icomeList.length; i++) {
                            const createdOn = this.icomeList[i].createdOn
                            this.icomeList[i].createdOn1 = moment(createdOn).format('YYYY-MM')
                        }
                    }
                    
                })
                .catch((error) => {
                }) 
        },
        defaultTime() { // 设置默认时间
            let newTime = new Date()
            this.pickerValue = newTime
            let timeDate = formatDate(newTime, this.yearshow)
            this.dataTime = timeDate.dataTime
            this.startTime = timeDate.startTime
            this.endTime = timeDate.endTime
        }
    },
    mounted () {
        this.getEvaluateList()
        this.getAllEvaluate()
    },
    created () {
        
        this.defaultTime()
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.income-statistics {
    .illness {
        padding: 0 10px;
        .illness-case {
            display: flex; 
            padding: 10px 0;
            color: $color-wfont;
            font-size: $font-l;
            p {
                flex: 1;
                text-align: center;
                &:after{ 
                    content: '';
                    width:0;
                    height:0;
                    vertical-align: middle;
                    margin: 0 0 2px 5px;
                    line-height: 0px;
                    display: inline-block;
                    border-width:5px 5px 0;
                    border-style:solid;
                    border-color: $color-wfont transparent transparent;}
            }
            .illness-label {
                width: 105px;
            }
        }
    }
    .set-ok {
        margin: 20px 10px;
    }
.flexList {display: flex; line-height: 20px; background: $color-wfont; text-align: center;}
.flexList > div {flex: 1;font-size: $font-l; position: relative;}
.flexList > div .mint-cell-allow-right::after {right: 5px;}
.income-top {background-color: $default-color;}   
.income-case {
    font-size: $font-l;
    padding-left: 10px;
    .smallFont {
        font-size: $font-m;
    }
    .income-time{
        font-size: $font-m;
    }
    .flex-left {
        text-align: left;
        .icome-name {
            position: relative;
            .hint { width: 18px; height: 18px; color: $color-wfont; background-color: $color-red; position: absolute; top:8px; left: -20px; font-size: $font-m;
display: inline-block; border-radius: 9px; line-height: 19px; text-align: center;}
        }
    }
    .flex-right {
        text-align: right;
    }
}   
.yaerAndMonth .picker-slot:nth-child(n+3){display: none;}
.yaerShow .picker-slot:nth-child(n+2){display: none;}     
.picker {position: absolute; bottom: 0; left: 0; right: 0; background: $color-wfont; z-index: 10;}
}

</style>