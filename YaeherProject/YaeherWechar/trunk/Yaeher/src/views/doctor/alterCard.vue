<template>
    <div class="alter-card padding-top">
        <mt-header fixed :title="title">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
            <div class="set-case">
                <p class="set-type">请绑定持卡人本人的银行卡</p>
                <mt-field class="noneAfter" label="户名" placeholder="" v-model="userName" readonly></mt-field>
                <mt-field label="卡号" placeholder="请输卡号" v-model="bankNo"></mt-field>
                <mt-field label="银行" placeholder="请输入银行" v-model="bankName"></mt-field>
                <mt-field label="所属支行" placeholder="请输入所属支行" v-model="subbranch"></mt-field>
                <div v-if="type === 'alter'" class="set-ok">
                    <mt-button @click="alterBank" type="primary" class="mint-button--large">保存</mt-button>
                </div>
                <div v-if="type === 'add'" class="set-ok">
                    <mt-button @click="addBank" type="primary" class="mint-button--large">添加</mt-button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { Toast } from 'mint-ui'
import { createSecret, verifyPrice } from 'assets/js/common.js'
export default {
    data () {
        return {
            id: 0,
            type: '',
            title: '',
            bankName: '',
            bankNo: '',
            userName: '',            
            subbranch: '',
            payMethod: ''
        }
    },
    methods: {
        getpayList() { // 获取收款方式列表
            this.instance.yaeherUserPaymentPageD({
            })
                .then((response) => {
                    const bankInfo1 = response.data.result.item.items
                    if (bankInfo1.length > 1) {
                        let bankInfo = bankInfo1[1]
                        this.bankNo = bankInfo.bankNo
                        this.bankName = bankInfo.bankName
                        this.subbranch = bankInfo.subbranch
                    }
                })
                .catch((error) => {
                }) 
        },
        addBank() { // 添加银行卡信息
            const bankNo = this.bankNo
            const bankName = this.bankName
            const subbranch = this.subbranch
            const payMethod = this.payMethod
            let verifyJson = [
                {
                    value: bankNo,
                    msg: '银行卡号不能为空'
                },
                {
                    value: bankName,
                    msg: '银行不能为空'
                },
                {
                    value: subbranch,
                    msg: '支行不能为空'
                }
            ]

            let verifyPriceFlg = verifyPrice(verifyJson)
            if (verifyPriceFlg === 1) return
            this.instance.createYaeherUserPaymentD({
                bankNo,
                bankName,
                subbranch,
                payMethod
            })
                .then((response) => {
                    Toast('添加成功')
                    this.$router.push({ 
                        path: '/payment-term'
                    })

                })
                .catch((error) => {
                }) 
        },
        alterBank() { // 添加银行卡信息
            const id = this.id
            const bankNo = this.bankNo
            const bankName = this.bankName
            const subbranch = this.subbranch
            const payMethod = this.payMethod
            let verifyJson = [
                {
                    value: bankNo,
                    msg: '银行卡号不能为空'
                },
                {
                    value: bankName,
                    msg: '银行不能为空'
                },
                {
                    value: subbranch,
                    msg: '支行不能为空'
                }
            ]

            let verifyPriceFlg = verifyPrice(verifyJson)
            if (verifyPriceFlg === 1) return
            this.instance.updateYaeherUserPaymentD({
                id,
                bankNo,
                bankName,
                subbranch,
                payMethod
            })
                .then((response) => {
                    Toast('修改成功')
                    this.$router.push({ 
                        path: '/payment-term'
                    })

                })
                .catch((error) => {
                }) 
        },
        getbankParams() { // 添加银行卡信息
            this.instance.yaeherUserPaymentTypeD({
            })
                .then((response) => {
                    this.payMethod = response.data.result.item[1].code
                    console.log(this.payMethod)
                })
                .catch((error) => {
                }) 
        },
    },
    mounted () {
        this.getbankParams()
        this.getpayList()
    },
    created () {
        this.userName = window.sessionStorage.getItem('userName')
        console.log(this.userName)
        this.type = this.$route.query.type
        if(this.type === 'add') {
            this.title = '添加银行卡'
        } else {
            this.title = '修改银行卡'
            this.id = parseInt(this.$route.query.id)
        }
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.alter-card {
    .set-type {background: $color-wfont; line-height: 40px; padding-left: 10px;}
    .mint-cell-text::after {content: '*'; color: $color-red;vertical-align: bottom; line-height: 18px; padding-left: 1px;}
    .noneAfter .mint-cell-text::after {content: '';}
    .set-ok {margin: 20px 10px;}
}

</style>