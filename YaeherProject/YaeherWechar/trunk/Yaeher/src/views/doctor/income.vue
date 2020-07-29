<template>
    <div class="income padding-top">
        <mt-header fixed title="我的收入">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
            <div class="income-top">
                <div class="illness" >
                    <div class="illness-case">
                        <!-- <p class="illness-label">搜索方式</p> -->
                        <p @click="selectSex" class="illness-value">{{searchType}}</p>
                        <p @click="openPicker" v-if="!show" class="illness-value">请选择时间</p>
                        <p @click="openPicker" v-if="show" class="illness-value">{{dataTime}}</p>
                    </div>
                </div>
                <!-- <div class="income-case">
                    <div class="illness" @click="openPicker">
                        <div class="illness-case">
                            <p class="illness-label">时间</p>
                            
                        </div>
                    </div>
                </div> -->
                <div class="total-income">
                    <b>685.00</b>
                    <div></div>
                    <span>总收入</span>
                </div>
            </div>
            <div>
                
                <ul>
                    <li class="flexList">
                        <div>日期</div>
                        <div>订单</div>
                        <div>收入</div>
                    </li>
                    <router-link tag="li" to="/income-list" class="flexList">
                        <div>04/01</div>
                        <div>2</div>
                        <div>100.00</div>
                    </router-link>
                    <router-link tag="li" to="/income-list" class="flexList">
                        <div>04/02</div>
                        <div>3</div>
                        <div>180.00</div>
                    </router-link>
                    <router-link tag="li" to="/income-list" class="flexList">
                        <div>04/03</div>
                        <div>4</div>
                        <div>210.00</div>
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
export default {
    data () {
        return {
            show: false,
            show1: false,
            yearshow: 0,
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
            this.dataTime=this.formatDate(this.$refs.picker.value)
            this.show = true
        },
        formatDate(date) {
            const yearshow = this.yearshow
            const y = date.getFullYear()
            let m = date.getMonth() + 1
            m = m < 10 ? '0' + m : m
            let d = date.getDate()
            d = d < 10 ? ('0' + d) : d
            if (yearshow===0) {
                return y + '-' + m + '-' + d
            } else if (yearshow===1) {
                return y + '-' + m 
            } else {
                return y
            }
            
        },
        getPickerValue() { 
            const selectArry = ['按日搜索', '按月搜索', '按年搜索'];    
            this.show1 = !this.show1
            this.searchType = this.$refs.picker1.getValues()[0]            
            this.yearshow = selectArry.indexOf(this.searchType)
        },
        selectSex() {
            this.show1 = !this.show1
        }
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.income {
    .income-top {
        .illness {
            padding: 0 10px;
            background: $default-color;
            .illness-case {
                display: flex; 
                padding: 10px 0;
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