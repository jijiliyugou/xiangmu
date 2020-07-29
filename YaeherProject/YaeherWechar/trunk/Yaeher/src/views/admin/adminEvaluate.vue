<template>
    <div class="admin-evaluate padding-top">
        <mt-header fixed title="评价统计">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="p-content index-control content listAbout">
            <div class="income-top">
                <div class="illness">
                    <div class="illness-case">
                        <p @click="selectSex(1)" class="illness-value">{{searchType}}</p>
                        <p @click="openPicker" v-if="!dataTime" class="illness-value">请选择时间</p>
                        <p @click="openPicker" v-if="dataTime" class="illness-value">{{dataTime}}</p>
                        <p @click="selectSex(2)" class="illness-value">{{starLevel}}</p>
                    </div>
                </div>
                <div class="total-income">
                    <b>{{evaluateAll.averageEvaluate}}</b>
                    <div>
                        <star :star=evaluateAll.averageEvaluate></star>
                    </div>
                    <p class="statistics">完成<span>{{evaluateAll.orderTotal}}</span>条订单，共收到<span>{{evaluateAll.evaluationToTal}}</span>条评论</p>
                    <div class="statistics-case">
                        <div class="star-statistics">
                            <star :star=5></star>
                            ：{{evaluateAll.fiveStar}}
                        </div>
                        <div class="star-statistics">
                            <star :star=4></star>
                            ：{{evaluateAll.fourStar}}
                        </div>
                        <div class="star-statistics">
                            <star :star=3></star>
                            ：{{evaluateAll.threeStar}}
                        </div>
                        <div class="star-statistics">
                            <star :star=2></star>
                            ：{{evaluateAll.twoStar}}
                        </div>
                        <div class="star-statistics">
                            <star :star=1></star>
                            ：{{evaluateAll.oneStar}}
                        </div>
                        <div class="star-statistics"><div class="give-star">无评价</div> ：{{evaluateAll.noEvaluationToTal}}</div>
                    </div>
                </div>
            </div>
            <div class="searchCase">
                <div class="searchBox">
                    <input @keypress="searchKeypress" ref="inputText" type="search" v-model="keyWord" placeholder="请输入医生名查找" />
                    <div @click="goSearch">搜索</div>
                </div>
            </div>
            <div>
                <ul v-infinite-scroll="getEvaluateList"
  				infinite-scroll-disabled="loading"
  				infinite-scroll-distance="10">
                    <router-link v-for="(item, index) in evaluateList" :key="index" tag="li" :to="{path: '/evaluate-detail', query: {typeEvaluate: 2, id: item.id, admin: 'admin'}}" class="evaluateList">
                        <div class="evaluateName">
                            <div>{{item.doctorName}}</div>
                            <div>患者：{{item.patientName}}</div>
                            <div>
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
        <mt-picker :slots="slots" ref="picker1" :showToolbar="true" v-show="show1">
            <div @click="selectSex(1)" class="slots-no">取消</div>
            <div @click="getPickerValue(1)" class="slots-ok">确认</div>
        </mt-picker>

        <mt-picker :slots="slots1" ref="picker2" :showToolbar="true" v-show="show2">
            <div @click="selectSex(2)" class="slots-no">取消</div>
            <div @click="getPickerValue(2)" class="slots-ok">确认</div>
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
import { Toast } from 'mint-ui';
import { formatDate } from 'assets/js/common.js'
let moment = require('moment');
import Star from 'components/star/star'
export default {
    components: {
      Star
    },
    data () {
        return {
            show: false,
            show1: false,
            show2: false,
            loading: true,
            typeEvaluate: 2,
            yearshow: 1,
            searchType: '按月搜索',
            starLevel: '全部星级',
            evaluateLevel: 0,
            keyWord: '',
            price: '',
            address: '',
            value: ['1'],
            value1: '1',
            value2: '1',
            pickerValue: '',
            startDate: new Date(2016, 0, 1),
            dataTime: '请选择时间',
            startTime: '',
            endTime: '',
            evaluateList: [],
            evaluateAll: {},
            skipCount: 1,
			maxResultCount: 10,
            totalPage: 2,
            slots: [
                {
                    flex: 1,
                    values: ['按日搜索', '按月搜索', '按年搜索'],
                    className: 'slot1',
                    textAlign: 'center'
                }
            ],
            slots1: [
                {
                    flex: 1,
                    values: ['全部星级', '一星', '二星', '三星', '四星', '五星'],
                    className: 'slot2',
                    textAlign: 'center'
                }
            ]
        }
    },
    methods: {
        openPicker() {
            this.$refs.picker.open();
        },
        searchKeypress(event) {
            if (event.keyCode == 13) {
                this.goSearch()
				//搜索
            } 
        },
        goSearch () {
            this.$refs.inputText.blur()
            this.getEvaluateList(true)
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
        getPickerValue(nub) { 
            if (nub === 1) {
                const selectArry = ['按日搜索', '按月搜索', '按年搜索'];    
                this.show1 = !this.show1
                this.searchType = this.$refs.picker1.getValues()[0]            
                this.yearshow = selectArry.indexOf(this.searchType)
            } else {
                const levelArray = ['全部星级', '一星', '二星', '三星', '四星', '五星'];    
                this.show2 = !this.show2
                this.starLevel = this.$refs.picker2.getValues()[0]    
                this.evaluateLevel = levelArray.indexOf(this.starLevel)
                this.getEvaluateList(true)
            }
            
        },
        getAllEvaluate() { // 获取总评价
            this.instance.qualityEvaluationTotalC({
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
            const keyWord = this.keyWord
            const evaluateLevel = this.evaluateLevel
            const maxResultCount  = this.maxResultCount 
            const skipCount = this.skipCount
            if (skipCount > this.totalPage) {
                if(skipCount > 2) {
                    Toast('没有更多了！')
                }
				return
			}
            this.instance.qualityEvaluationListC({
                startTime,
                endTime,
                keyWord,
                evaluateLevel,
                skipCount,
                maxResultCount
            })
                .then((response) => {
                    setTimeout(() => {
                        this.loading = false
                        if (flag) this.evaluateList = []
                        let moreFlag = response.data.result.item.items
                        if (!moreFlag || moreFlag.length===0) {
							this.loading = true
						}
                        this.evaluateList = this.evaluateList.concat(response.data.result.item.items)
                        this.skipCount ++
                        this.totalPage = response.data.result.item.totalPage
                        if (this.evaluateList.length>0) {
                            for (let i = 0; i < this.evaluateList.length; i++) {
                                this.evaluateList[i].createdOn = moment(this.evaluateList[i].createdOn).format('YYYY-MM-DD HH:mm:ss')
                            }
                        } else {
                            Toast('暂无数据')
                        }
                    }, 100);
                    
                })
                .catch((error) => {
                    this.loading = true
                }) 
        },
        selectSex(nub) {
            if (nub === 1) {
                this.show1 = !this.show1
            } else {
                this.show2 = !this.show2
            }
            
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
        
    },
    created () {
        this.defaultTime()
        
    },
    activated () {
        this.selected = 'nav1'
        this.getAllEvaluate()
        this.getEvaluateList()
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";
.admin-evaluate {
    .listAbout {
        position: absolute; left: 0; right: 0; bottom: 0px;top: 42px;
    }
    .index-control {
        .income-top {
            background-color: $default-color;
            .illness {
                padding: 0 10px;
                background: $default-color;
                .illness-case {
                    display: flex; 
                    padding: 0px 0;
                    color: $color-wfont;
                    font-size: $font-m;
                    p {
                        flex: 1;
                        text-align: center;
                        &:after{ 
                            content: '';
                            width:0;
                            height:0;
                            vertical-align: middle;
                            margin-left: 5px;
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
                .statistics {
                    font-size: $font-m;
                    line-height: 20px;
                    span {
                        font-size: $font-l;
                        padding: 0 3px;
                    }
                }
                .statistics-case {
                    text-align: left;
                    padding-left: 83px;
                    .star-statistics {
                        font-size: $font-l;
                        line-height: 20px;
                    }
                }
                
            }
        }
    }

    .set-ok {
        margin: 20px 10px;
    }


    .yaerAndMonth .picker-slot:nth-child(n+3){display: none;}
    .yaerShow .picker-slot:nth-child(n+2){display: none;}     
    .picker {position: absolute; bottom: 0; left: 0; right: 0; background: $color-wfont; z-index: 10;}

    .evaluateList {
        .evaluateName {
            display: flex; 
            font-size: $font-m;
            div {
                flex: 1; 
                line-height: 16px;
            }
        }
        .eavaluateArticle {
            color: $color-bfont;
            font-size: $font-m;
            overflow: hidden; 
		    white-space: nowrap;
    	    text-overflow: ellipsis;
        }
        .evaluateTime {
            font-size: $font-m; 
            padding: 5px 0 0;
            .controlStar {
                line-height: 24px;
                .floatRight {
                    float: right;
                }
            }
        }

    }
}




</style>