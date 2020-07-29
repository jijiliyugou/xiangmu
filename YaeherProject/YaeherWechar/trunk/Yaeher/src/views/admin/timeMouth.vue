<template>
    <div class="timeMouth padding-top">
        <mt-header fixed title="收入统计">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
            <ul>
                <li class="flexList">
                    <div>时间</div>
                    <div>收入流水</div>
                    <div>公司进账</div>
                </li>
                <router-link tag="li"  v-for="(item, index) in icomeList" :key="index" :to="{path: '/time-day', query: {timeValue: item.createdOn}}" class="flexList">
                    <div>{{item.createdOn1}}</div>
                    <div>{{item.incomeTotal}}</div>
                    <div>
                        {{item.incomeWater}}
                        <i class="mint-cell-allow-right"></i>
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
            const incomeType = 'month'
            this.instance.adminIncomeDetailReportC({
                startTime,
                endTime,
                incomeType,
            })
                .then((response) => {
                    this.icomeList = response.data.result.item
                    for (let i = 0; i < this.icomeList.length; i++) {
                        const createdOn = this.icomeList[i].createdOn
                        this.icomeList[i].createdOn1 = moment(createdOn).format('MM-DD')
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
        
        newTime = newTime.substring(0,8)
        console.log(newTime)
        this.startTime = `${newTime}01`
        console.log(this.startTime)

        let y = parseInt(newTime.substr(0,4))
        let m = parseInt(newTime.substr(5,2))
        let lastDay  = new Date(y, m, 0)
        lastDay = lastDay.getDate()

        this.endTime = `${y}-${m}-${lastDay}`
        console.log(this.endTime)
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";
.timeMouth {
    .flexList {display: flex; line-height: 20px; background: $color-wfont; text-align: center;}
    .flexList > div {flex: 1;font-size: $font-l; position: relative;}
    .flexList > div .mint-cell-allow-right::after {right: 5px;}
}

</style>