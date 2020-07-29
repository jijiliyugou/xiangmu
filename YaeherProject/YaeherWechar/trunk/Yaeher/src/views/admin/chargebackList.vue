<template>
    <div class="chargeback-list padding-top">
        <mt-header fixed title="退单列表">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>   
        <div class="content listAbout">
            <ul v-infinite-scroll="getOrderList"
  				infinite-scroll-disabled="loading"
  				infinite-scroll-distance="10" class="chargebackList">
                <router-link  v-for="(item, index) in recordList" :key="index" tag="li" :to="{path: '/chargeback-detail', query: {id: item.id}}" class="chargecack-flex">
                    <div class="chargeback-title">
                        <p>退单原因：</p>
                        <p>退单原因描述：</p>
                        <p>退单时间：</p>
                    </div>
                    <div class="chargeback-content">
                        <span v-if="item.checkState ==='checking'" class="hint red-hint">待审核</span>
                        <span v-if="item.checkState ==='fail'" class="hint red-hint">不通过</span>
                        <span v-if="item.checkState ==='success'" class="hint green-hint">已通过</span>
                        <p>{{item.refundReason1}}</p>
                        <p>{{item.refundRemarks}}</p>
                        <p>{{item.createdOn}}</p>
                        <i class="mint-cell-allow-right"></i>
                    </div>    
                </router-link>
            </ul>
        </div> 
    </div>
</template>

<script>
import { MessageBox, Toast } from 'mint-ui';
let moment = require('moment');
export default {
    data () {
        return {
            skipCount: 1,
			maxResultCount: 10,
			totalPage: 2,
            recordList: [],
            loading: true
        }
    },
    methods: {
        getOrderList() { // 获取咨询列表
            this.loading = true
            const maxResultCount  = this.maxResultCount 
            const skipCount = this.skipCount
            if (skipCount > this.totalPage) {
				if(skipCount > 2) {
                    Toast('没有更多了！')
                }
				return
			}
            this.instance.refundManagePageA({
                maxResultCount,
                skipCount
            })
                .then((response) => {
                    setTimeout(() => {
                        this.loading = false
                        let moreFlag = response.data.result.item.items
                        if (!moreFlag || moreFlag.length===0) {
							this.loading = true
                        }
                        this.recordList = this.recordList.concat(response.data.result.item.items)
                        console.log(this.recordList)
                        for (let i = 0; i < this.recordList.length; i++) {
                            this.recordList[i].createdOn = moment(this.recordList[i].createdOn).format('YYYY-MM-DD HH:mm:ss')
                            if (this.recordList[i].refundReason) {
                                this.recordList[i].refundReason1 = JSON.parse(this.recordList[i].refundReason).LabelName
                            }    
                        }
                        console.log(this.skipCount)
                        this.skipCount ++
                        this.totalPage = response.data.result.item.totalPage
                        console.log(this.skipCount, this.totalPage)
                    }, 100);    
                })
                .catch((error) => {
                    this.loading = true
                }) 
        }        
    },
    mounted () {
        this.getOrderList()
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.chargeback-list {
    .listAbout {
        position: absolute; left: 0; right: 0; bottom: 0px;top: 42px;
    }
    .chargecack-flex {
        display: flex;
        font-size: $font-l;
        .chargeback-title {
            width: 100px;
        }
        .chargeback-content {
            position: relative;
            flex: 1;
            .hint {
                position: absolute;
                top: 0;
                right: 0;
            }
            p {
                width: 200px;
                height: 19px;
                overflow: hidden;
                white-space: nowrap;
                text-overflow: ellipsis;
            }
            .mint-cell-allow-right::after {
                right: 0;
            }
        }
    }
}

</style>