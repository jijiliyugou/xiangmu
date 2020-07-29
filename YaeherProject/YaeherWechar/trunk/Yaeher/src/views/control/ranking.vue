<template>
    <div class="ranking padding-top">
        <mt-header fixed title="医生排行">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content listAbout">
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
                
                <ul  v-infinite-scroll="getEvaluateList"
  				infinite-scroll-disabled="loading"
  				infinite-scroll-distance="10">
                    <li class="flexList">
                        <div>医生</div>
                        <div>订单</div>
                        <div>收入</div>
                    </li>
                    <li v-for="(item, index) in evaluateList" :key="index" class="flexList">
                        <div class="text-left">{{index+1}}.{{item.doctorName}}</div>
                        <div>{{item.orderTotal}}</div>
                        <div class="text-right">{{item.revenueTotal}}</div>
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
import { Tabbar, TabItem, Toast } from 'mint-ui';
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
            evaluateList: [],
            pickerValue: '',
            startDate: new Date(2016, 0, 1),
            dataTime: '',
            totalType: 'day',
            slots: [
                {
                    flex: 1,
                    values: ['按日搜索', '按月搜索', '按年搜索'],
                    className: 'slot1',
                    textAlign: 'center'
                }
            ],
            skipCount: 1,
			maxResultCount: 10,
            totalPage: 2,
            loading: true
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
            this.getEvaluateList(true)
        },
        getPickerValue() { 
            const selectArry = ['按日搜索', '按月搜索', '按年搜索'];    
            this.show1 = !this.show1
            this.searchType = this.$refs.picker1.getValues()[0]            
            this.yearshow = selectArry.indexOf(this.searchType)
        },
        getEvaluateList(flag) { // 获取医生排行列表
            if (flag) {
                this.skipCount = 1
                this.totalPage = 2
            }
            const maxResultCount  = this.maxResultCount 
            const skipCount = this.skipCount
            const startTime = this.startTime
            const endTime = this.endTime
            const totalType = ['day', 'month', 'year'][this.yearshow]
            console.log(skipCount, this.totalPage)
            if (skipCount > this.totalPage) {
				if(skipCount > 2) {
                    Toast('没有更多了！')
                }
				return
			}
            this.instance.qualityDoctorRankingC({
                startTime,
                endTime,
                totalType,
            })
                .then((response) => {
                    if (response.data.result.code === 200) {
                        setTimeout(() => {
                            this.loading = false
                            let moreFlag = response.data.result.item.items
                            
                            if (!moreFlag || moreFlag.length===0) {
                                this.loading = true
                            }
                            if (flag) this.recordList = []
                            this.evaluateList = this.evaluateList.concat(response.data.result.item.items)
                            this.skipCount ++
                            this.totalPage = response.data.result.item.totalPage
                            console.log(this.skipCount, this.totalPage)
                        }, 100);   
                    } else {
                        this.loading = true
                    }
                })
                .catch((error) => {
                    this.loading = true
                }) 
        },
        selectSex() {
            this.show1 = !this.show1
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

.ranking {
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
    .flexList .text-left {text-align: left;}
    .flexList .text-right {text-align: right;}
    .income-top {background-color: $default-color;}   
    .total-income {color: $color-wfont; padding: 0 20px 20px; text-align: center;}
    .total-income b {font-size: 24px; line-height: 32px; font-weight: 600;}
    .total-income span {font-size: $font-m;}
    .yaerAndMonth .picker-slot:nth-child(n+3){display: none;}
    .yaerShow .picker-slot:nth-child(n+2){display: none;}     
    .picker {position: absolute; bottom: 0; left: 0; right: 0; background: $color-wfont; z-index: 10;}
}

</style>