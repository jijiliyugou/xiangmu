<template>
    <div class="payment-term padding-top">
        <mt-header fixed title="收款方式">
            <a @click="$router.push({path: '/doctor-user'})" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
            <mt-radio
            title="收款方式"
            v-model="value"
            :options="dataList"
            >
            </mt-radio>
            <ul class="payment-list">
                <li v-if="payLength >= 1">我的微信：{{wxName}}</li>
                <router-link v-if="payLength === 1" class="bankCard" tag="li" :to="{path: '/alter-card', query: {type: 'add'}}">
                    我的银行卡：无(去绑定)
                    <i class="mint-cell-allow-right"></i>
                </router-link>
                <router-link v-if="payLength > 1" class="bankCard" tag="li" :to="{path: '/alter-card', query: {type: 'alter', id}}">
                    我的银行卡：{{bankNo}}
                    <i class="mint-cell-allow-right"></i>
                </router-link>
            </ul>
            <div class="term-ok">
                <mt-button @click="alterPayType" type="primary" class="mint-button--large">保存</mt-button>
            </div>
        </div>
        
    </div>
</template>

<script>
import { Toast } from 'mint-ui'
import { createSecret } from 'assets/js/common.js'
export default {
    data () {
        return {
            value: '1',
            payList: [],
            slotsArry: [],
            payLength: 0,
            bankNo: '',
            wxName: '',
            id: 0
        }
    },
    methods: {
        getpayList() { // 获取收款方式列表
            this.instance.yaeherUserPaymentPageD({
            })
                .then((response) => {
                    let _this = this
                    this.payList = response.data.result.item.items
                    this.payLength = this.payList.length
                    this.wxName = this.payList[0].paymentAccout
                    if (this.payLength > 1) {
                        this.bankNo = this.payList[1].bankNo
                        this.id = this.payList[1].id
                    }
                    
                    for(var j = 0;j < this.payList.length;j++ ) {
                        
                        let label = this.payList[j].payMethodName
                        let value = this.payList[j].payMethod
                        if (this.payList[j].isDefault) {
                            this.value = value
                        }
                        let obj = {
                            label,
                            value
                        }
                        this.slotsArry.push(obj)
                    }
                    if (this.payLength===1) {
                        let label = '银行卡收款'
                        let value = 0
                        let disabled = true
                        let obj = {
                            label,
                            value,
                            disabled
                        }
                        this.slotsArry.push(obj)
                    }
                })
                .catch((error) => {
                }) 
        },
        alterPayType() { // 修改收款方式
            const value = this.value
            let id = 0
            for(var j = 0;j < this.payList.length;j++ ) {
                if (this.payList[j].payMethod === value) {
                    id = this.payList[j].id
                }
            }
            this.instance.updateYaeherUserPaymentD({
                id,
                isDefault: 1
            })
                .then((response) => {
                    Toast('切换成功')
                    this.$router.push({ 
                        path: '/doctor-user'
                    })
                })
                .catch((error) => {
                }) 
        }
    },
    mounted () {
        this.getpayList()
    },
    computed: {
        dataList() {
            let dataSlots = this.slotsArry
            return dataSlots
        }
    },
    created () {
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.payment-term {
    .payment-list {
        margin-top: 10px;
        .bankCard {
            position: relative;
            .mint-cell-allow-right::after {
                right: 10px;
            }
        }
    }
    .term-ok {margin: 20px 10px;}
}

</style>