<template>
    <div class="flow-statistics padding-top">
        <mt-header fixed title="流量查看">
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
                        总用户数：{{charkDetail.totalUser}}
                    </li>
                    <li class="flexList">
                        付费用户数：{{charkDetail.paidUser}}
                    </li>
                    <li class="flexList">
                        新增用户数：{{charkDetail.newUser}}
                    </li>
                    <li class="flexList">
                        新增医生数：{{charkDetail.newDoctor}}
                    </li>
                    <li class="flexList">
                        新增付费用户数：{{charkDetail.newPaidUser}}
                    </li>
                    <li class="flexList">
                        复购用户数：{{charkDetail.newRepurchaseCount}}
                    </li>
                    <li class="flexList">
                        复购率：{{charkDetail.newRepurchaserate}}%
                    </li>
                    
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
export default {
    data () {
        return {
            show: false,
            show1: false,
            yearshow: 1,
            searchType: '按月搜索',
            price: '',
            address: '',
            value: ['1'],
            value1: '1',
            value2: '1',
            charkDetail: {},
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
           let timeObj = formatDate(this.$refs.picker.value, this.yearshow)
            console.log(timeObj)
            this.dataTime=timeObj.dataTime
            this.startTime = timeObj.startTime
            this.endTime = timeObj.endTime
            this.show = true
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
        getEvaluateList() { // 获取医生排行列表
            const startTime = this.startTime
            const endTime = this.endTime
            const totalType = ['day', 'month', 'year'][this.yearshow]
            this.instance.adminFlowReportA({
                startTime,
                endTime,
                totalType,
            })
                .then((response) => {
                    this.charkDetail = response.data.result.item
                    console.log(this.charkDetail)
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
    },
    created () {
        this.defaultTime()
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.flow-statistics {
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
    .flexList > div {flex: 1;}
    .income-top {background-color: $default-color;}   
    .total-income {color: $color-wfont; padding: 0 20px 20px; text-align: center;}
    .total-income b {font-size: 24px; line-height: 32px; font-weight: 600;}
    .total-income span {font-size: $font-m;}
    .yaerAndMonth .picker-slot:nth-child(n+3){display: none;}
    .yaerShow .picker-slot:nth-child(n+2){display: none;}     
    .picker {position: absolute; bottom: 0; left: 0; right: 0; background: $color-wfont; z-index: 10;}
}

</style>