<template>
    <div class="evaluateDoctor padding-top">
        <mt-header fixed title="我的评价">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div ref="srcollBox" class="content listAbout">
            <div class="income-top">
                <div class="illness" >
                    <div class="illness-case">
                        <!-- <p @click="selectSex" class="illness-value">{{searchType}}</p> -->
                        <p @click="openPicker" v-if="!dataTime" class="illness-value">请选择时间</p>
                        <p @click="openPicker" v-if="dataTime" class="illness-value">{{dataTime}}</p>
                    </div>
                </div>
                <div class="total-income">
                    <b>{{evaluateAll.averageEvaluate}}</b>
                    <div>
                        <star :star="evaluateAll.averageEvaluate"></star>
                    </div>
                    <span>总订单：{{evaluateAll.receiptNumBer}}</span>
                </div>
            </div>
            <div>
                <ul v-infinite-scroll="getEvaluateList"
  				infinite-scroll-disabled="loading"
  				infinite-scroll-distance="10" class="evaluate-list">
                    <router-link v-for="(item, index) in evaluateList" :key="index" tag="li" :to="{path: '/evaluate-detail', query: {typeEvaluate: 3, id: item.id}}" class="evaluateList">
                        <div class="evaluateName">
                            <div>{{item.doctorName}}</div>
                            <div>患者：{{item.patientName}}</div>
                            <div class="evalStar">
                                <star :star="item.evaluateLevel"></star>
                            </div>
                        </div>
                        <p class="eavaluateArticle">
                            {{item.evaluateContent}}
                        </p>
                        <div v-if="item.isQuality" class="evaluateTime">
                            
                            <div class="controlStar">
                                质控评分：<star :star="item.qualityLevel"></star>
                                <span class="floatRight">{{item.createdOn}}</span>
                            </div>
                            <p class="eavaluateArticle">
                                {{item.qualityReason}}
                            </p>
                        </div>
                    </router-link>
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
import Star from 'components/star/star'
import { Toast } from 'mint-ui'
let moment = require('moment');
export default {
    components: {
      Star
    },
    data () {
        return {
            show: false,
            show1: false,
            yearshow: 0,
            evaluateList: [],
            evaluateAll: {},
            lastDay: '',
            startTime: '',
            endTime: '',
            pickerValue: '',
            startDate: new Date(2016, 0, 1),
            dataTime: '',
            skipCount: 1,
			maxResultCount: 10,
            totalPage: 2,
            loading: true,
            homeTop: 0
        }
    },
    methods: {
        openPicker() {
            this.$refs.picker.open();
        },
        handleConfirm() {

            this.dataTime=this.formatDate(this.$refs.picker.value)
            this.startTime = `${this.dataTime}-01`
            this.endTime = `${this.dataTime}-${this.lastDay}`
            this.show = true
            this.getEvaluateList(true)
        },
        formatDate(date) {
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
        getAllEvaluate() { // 获取总评价
            this.instance.consultationEvaluationDetailD({
            })
                .then((response) => {
                    this.evaluateAll = response.data.result.item
                    
                })
                .catch((error) => {
                }) 
        },
        getEvaluateList(flag) { // 获取评价列表、查询评价列表
            this.loading = true
            if (flag) {
                this.skipCount = 1
                this.totalPage = 2
            }
            const startTime = this.startTime
            const endTime = this.endTime
            const maxResultCount  = this.maxResultCount 
            const skipCount = this.skipCount
            if (skipCount > this.totalPage) {
				if(skipCount > 2) {
                    Toast('没有更多了！')
                }
				return
			}
            this.instance.consultationEvaluationPageD({
                startTime,
                endTime,
                skipCount,
                maxResultCount
            })
                .then((response) => {
                    setTimeout(() => {
                        this.loading = false
                        let moreFlag = response.data.result.item.items
                        if (!moreFlag || moreFlag.length===0) {
							this.loading = true
                        }
                        if (flag) this.evaluateList = []
                        this.evaluateList = this.evaluateList.concat(response.data.result.item.items)
                        this.skipCount ++
                        this.totalPage = response.data.result.item.totalPage
                        if (this.evaluateList) {
                            for (let i = 0; i < this.evaluateList.length; i++) {
                                this.evaluateList[i].createdOn = moment(this.evaluateList[i].createdOn).format('YYYY-MM-DD HH:mm:ss')
                            }
                        }
                    }, 100);
                    
                })
                .catch((error) => {
                    this.loading = true
                }) 
        },
        selectSex() {
            this.show1 = !this.show1
        }
    },
    mounted () {
        this.getEvaluateList()
    },
    created () {
        let newTime = new Date()
        this.pickerValue = newTime
        let timeDate = this.formatDate(newTime)
        this.dataTime = timeDate
        this.startTime = `${this.dataTime}-01`
        this.endTime = `${this.dataTime}-${this.lastDay}`
        this.getAllEvaluate()
    },
    activated () {
        this.$refs.srcollBox.scrollTop = this.homeTop || 0
    },
    beforeRouteLeave (to, from, next) {
        let scrollA = this.$refs.srcollBox
        this.homeTop = scrollA.scrollTop || 0
        console.log(this.homeTop )
        next()
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.evaluateDoctor {
    .listAbout {
        position: absolute; left: 0; right: 0; bottom: 0px;top: 42px;
    }
    .income-top {
        background-color: $default-color;
        .illness {
            padding: 0 10px;
            background: $default-color;
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
        .total-income {
            color: $color-wfont; 
            padding: 0 20px 20px; 
            text-align: center;
            b {
                font-size: 24px; 
                line-height: 32px; 
                font-weight: 600;
            }
            span {
                font-size: $font-m;
            }
        }
    }
    .set-ok {
        margin: 20px 10px;
    }


    .yaerAndMonth .picker-slot:nth-child(n+3){display: none;}
    .yaerShow .picker-slot:nth-child(n+2){display: none;}     
    .picker {position: absolute; bottom: 0; left: 0; right: 0; background: $color-wfont; z-index: 10;}
    
    .evaluate-list {
        min-height: 500px;
    }
    .evaluateList {
        .evaluateName {
            display: flex; 
            font-size: $font-m;
            div {
                flex: 1; 
                line-height: 16px;
            }
            .evalStar {
                text-align: right;
            }
        }
        .eavaluateArticle {
            color: $color-bfont;
            font-size: $font-m;
            white-space: nowrap; 
            overflow: hidden; 
            text-overflow: ellipsis;
        }
        .evaluateTime {
            font-size: $font-m; 
            padding: 5px 0 0;
            .controlStar {
                .floatRight {
                    float: right;
                }

            }
            .eavaluateArticle {
                width: 230px; 
                white-space: nowrap; 
                overflow: hidden; 
                text-overflow: ellipsis;
            }
        }

    }
}




</style>