<template>
    <div class="timeDay padding-top">
        <mt-header fixed title="收入统计">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
            <ul>
                <router-link v-for="(item, index) in icomeList" :key="index" tag="li" :to="{path: '/order-look', query: {id: item.consultID}}" class="flexList income-case">
                    <div class="flex-left">
                        <p class="icome-name"> 
                            <span v-if="item.consultType === 'ImageText'" class="hint">文</span>
                            <span v-if="item.consultType === 'Phone'"  class="hint">电</span>
                            <span>{{item.doctorName}}</span> 
                            <span class="smallFont">患者：{{item.patientName}}</span>
                        </p>
                        <p class="income-time smallFont">{{item.createdOn}}</p>
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
</template>

<script>
import { formatDate } from 'assets/js/common.js'
let moment = require('moment');
export default {
    data () {
        return {
            icomeList: [],
            timeValue: '',
            startTime: '',
            endTime: ''
        }
    },
    methods: {
        getEvaluateList() { // 获取收入明细
            const startTime = this.startTime
            const endTime = this.endTime
            const incomeType = 'day'
            this.instance.adminIncomeDetailReportC({
                startTime,
                endTime,
                incomeType,
            })
                .then((response) => {
                    this.icomeList = response.data.result.item
                    for (let i = 0; i < this.icomeList.length; i++) {
                        const createdOn = this.icomeList[i].createdOn
                        this.icomeList[i].createdOn = moment(createdOn).format('HH:mm:ss')
                    }
                    
                })
                .catch((error) => {
                }) 
        }
    },
    mounted () {
        this.getEvaluateList()
    },
    created () {
        let newTime = this.$route.query.timeValue
        newTime = newTime.substring(0,10)
        this.startTime = newTime
        this.endTime = newTime
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";
.timeDay {
    .flexList {display: flex; line-height: 20px; padding-left: 15px; background: $color-wfont; text-align: center;}
    .flexList > div {flex: 1; font-size: $font-l;}
    .smallFont {font-size: $font-m;}
    .flex-left {text-align: left;}
    .icome-name {position: relative;}
    .flex-right {text-align: right;}
    .icome-name .hint { width: 18px; height: 18px; color: $color-wfont; background-color: $color-red; position: absolute; top:8px; left: -23px; font-size: $font-m;
	display: inline-block; border-radius: 9px; line-height: 19px; text-align: center;}
}

</style>