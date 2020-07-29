<template>
    <div class="income-list padding-top">
        <mt-header fixed title="收入明细">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content listAbout">
            <ul v-infinite-scroll="getAllEvaluate"
  				infinite-scroll-disabled="loading"
  				infinite-scroll-distance="10" class="incomeList">
                <router-link  v-for="(item, index) in ordertotallist" :key="index" tag="li" :to="{path: '/order-look', query: {id: item.consultID}}" class="flex"> 
                    <div class="flex-left">
                        <p class="flex-name">{{item.patientName}}</p>
                        <p class="doctor-name">{{item.doctorName}}医生</p>
                        <p>{{item.createdOn}}</p>
                        <div class="typeIcon">
                            <img src="../../assets/image/chat-icon.png" />
                        </div>
                    </div>
                    <div class="flex-right">
                        <span v-if="item.orderMoney > 0" class="green-hint">+{{item.orderMoney}}</span>
                        <span v-if="item.orderMoney < 0" class="red-hint">{{item.orderMoney}}</span>
                    </div>
                </router-link>
                
            </ul>
        </div>
    </div>
</template>

<script>
import { Toast } from 'mint-ui';
let moment = require('moment');
export default {
    data () {
        return {
            starttime: '',
            endtime: '',
            skipCount: 1,
			maxResultCount: 10,
			totalPage: 2,
            ordertotallist: [],
            loading: true
        }
    },
    methods: {
        getAllEvaluate() { // 获取收入排行明细
            this.loading = true
			const skipCount = this.skipCount
			const maxResultCount = this.maxResultCount
			if (skipCount > this.totalPage) {
				if(skipCount > 2) {
                    Toast('没有更多了！')
                }
				return
			}
            const starttime = this.starttime
            const totalType = 'day'
            const endtime = this.endtime
            this.instance.orderManageListD({
                totalType,
                starttime,
                endtime,
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
                        let ordertotallist1 = response.data.result.item.items
                        this.ordertotallist = this.ordertotallist.concat(ordertotallist1)
                        this.skipCount ++
                        this.totalPage = response.data.result.item.totalPage
                        for (let i = 0; i < this.ordertotallist.length; i++) {
                            const createdOn = this.ordertotallist[i].createdOn
                            this.ordertotallist[i].createdOn = moment(this.ordertotallist[i].createdOn).format('YYYY-MM-DD HH:mm:ss')
                        }
                    }, 100);    
                })
                .catch((error) => {
                    this.loading = true
                }) 
        }
    },
    mounted () {
        this.getAllEvaluate()
    },
    created () {
        this.starttime = moment(this.$route.query.starttime).format('YYYY-MM-DD')
        this.endtime = moment(this.$route.query.endtime).format('YYYY-MM-DD')
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.income-list {
    .listAbout {position: absolute; left: 0; right: 0; bottom: 0px;top: 42px;}
    .incomeList {
        margin-top: 10px;
        .flex-left {
            padding-left: 30px; 
            position: relative;
            p {
                font-size: $font-m;
            }
            .flex-name {
                font-size: $font-l;
                padding-top: 5px;
            }
            .doctor-name {
                font-size: $font-m;
                color: $color-bfont;
            }
            .typeIcon {
                position: absolute;
                top: 10px;
                left: 0px;
                width: 25px;
                height: 25px;
                img {
                    display: block;
                    width: 100%;
                }
            }
        }
        .flex-right {
            color: $color-green;
            display: flex;
            text-align: right;
            align-items: center;
            span {
                display: block;
                width: 100%;
                text-align: right;
            }
        }
    }
}


</style>