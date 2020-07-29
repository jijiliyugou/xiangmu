<template>
    <div class="ranking padding-top">
        <mt-header fixed title="我的收入">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
            <div class="income-top">
                <div class="illness" >
                    <div class="illness-case">
                        <p @click="openPicker" class="illness-value">{{dataTime}}</p>
                    </div>
                </div>
                <div class="total-income">
                    <b v-if="show">{{icomeDetail.completeMoney}}</b>
                    <b v-if="!show">0.00</b>
                    <div></div>
                    <span>本月收入( 元)</span>
                </div>
            </div>
            <div>              
                <ul>
                    <li class="flexList">
                        <div>订单数</div>
                        <div>总排名</div>
                        <div>医生%</div>
                    </li>
                    <li v-if="show" class="flexList">
                        <div>{{icomeDetail.completeTotal}}</div>
                        <div>{{icomeDetail.orderRanking}}</div>
                        <div>前{{icomeDetail.orderPoint}}</div>
                    </li>
                    <li v-if="!show" class="flexList">
                        <div>\</div>
                        <div>\</div>
                        <div>\%</div>
                    </li>
                    <li class="flexList">
                        <div>收入</div>
                        <div>总排名</div>
                        <div>医生%</div>
                    </li>
                    <li v-if="show"  class="flexList">
                        <div>{{icomeDetail.completeMoney}}</div>
                        <div>{{icomeDetail.revenueRanking}}</div>
                        <div>前{{icomeDetail.revenuePoint}}</div>
                    </li>
                    <li v-if="!show" class="flexList">
                        <div>\</div>
                        <div>\</div>
                        <div>\%</div>
                    </li>
                </ul>
            </div>
            <div>
                <div class="iconmeTitle">收入明细</div>
                <ul>
                    <li class="flexList">
                        <div>日期</div>
                        <div>订单</div>
                        <div>收入</div>
                    </li>
                    <router-link v-if="show" v-for="(item, index) in ordertotallist" :key="index" tag="li" :to="{path: '/income-list', query: {starttime: item.totalDate, endtime: item.totalDate}}" class="flexList">
                        <div>{{item.totalDate1}}</div>
                        <div>{{item.completeTotal}}</div>
                        <div>{{item.completeMoney}}</div>
                    </router-link>
                    <li v-if="!show" class="flexList">
                        <div>\</div>
                        <div>\</div>
                        <div>\</div>
                    </li>
                </ul>
            </div>

            
        </div>
        <mt-datetime-picker
            class="yaerAndMonth"
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
import { Toast } from 'mint-ui';
let moment = require('moment');
export default {
    data () {
        return {
            show: false,
            yearshow: 1,
            pickerValue: '',
            startDate: new Date(2016, 0, 1),
            dataTime: '请选择时间',
            lastDay: '',
            starttime: '',
            endtime: '',
            icomeDetail: {},
            ordertotallist: []
        }
    },
    methods: {
        openPicker() {
            this.$refs.picker.open();
        },
        handleConfirm() {
            this.dataTime=this.formatDate(this.$refs.picker.value)
            console.log(this.dataTime)
            this.starttime = `${this.dataTime}-01`
            this.endtime = `${this.dataTime}-${this.lastDay}`
            this.getAllEvaluate()
        },
        formatDate(date) {
            console.log(date)
            const yearshow = this.yearshow
            const y = date.getFullYear()
            let m = date.getMonth() + 1
            m = m < 10 ? '0' + m : m
            let d = date.getDate()
            d = d < 10 ? ('0' + d) : d
            let lastDay  = new Date(y, m, 0)
            this.lastDay = lastDay.getDate()
            return y + '-' + m 
            
        },
        getAllEvaluate() { // 获取收入排行
            const totalType = 'month'
            const starttime = this.starttime
            const endtime = this.endtime
            console.log(starttime)
            console.log(endtime)
            this.instance.revenueTotalD({
                totalType,
                starttime,
                endtime
            })
                .then((response) => {
                    let icomeDetail = response.data.result.item
                    if (icomeDetail) {
                        this.show = true
                        this.icomeDetail = icomeDetail
                        this.ordertotallist =  this.icomeDetail.ordertotallist
                        for (let i = 0; i < this.ordertotallist.length; i++) {
                            const createdOn = this.ordertotallist[i].totalDate
                            this.ordertotallist[i].totalDate1 = moment(createdOn).format('MM-DD')
                        }
                    } else {
                        this.show = false
                        Toast('暂无数据')
                    }
                    
                })
                .catch((error) => {
                }) 
        }
    },
    mounted () {
        

    },
    created () {
        let newTime = new Date()
        this.pickerValue = newTime
        let timeDate = this.formatDate(newTime)
        this.dataTime = timeDate
        this.starttime = `${this.dataTime}-01`
        this.endtime = `${this.dataTime}-${this.lastDay}`
    },
    activated() {
        
        this.getAllEvaluate()
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.ranking {
    .illness {
        padding: 0 10px;
        .illness-case {
            display: flex; 
            padding: 5px 0;
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
                    margin-left: 5px;
                    margin-bottom: 2px;
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
    .iconmeTitle {text-align: center; padding: 10px;}
    .income-top {background-color: $default-color;}   
    .total-income {color: $color-wfont; padding: 0 20px 20px; text-align: center;}
    .total-income b {font-size: 24px; line-height: 32px; font-weight: 600;}
    .total-income span {font-size: $font-m;}
    .yaerAndMonth .picker-slot:nth-child(n+3){display: none;}
    .yaerShow .picker-slot:nth-child(n+2){display: none;}     
    .picker {position: absolute; bottom: 0; left: 0; right: 0; background: $color-wfont; z-index: 10;}
}

</style>