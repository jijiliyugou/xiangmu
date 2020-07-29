<template>
    <div class="set-server padding-top">
        <mt-header fixed :title="title">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
            <div class="set-case">
                <p v-if="type === 'ImageText'" class="set-type">图文咨询</p>
                <p v-if="type === 'Phone'" class="set-type">电话咨询</p>
                <mt-field label="价格" type="number" placeholder="请输入每次的价格（元）" v-model="serviceExpense"></mt-field>
                <mt-field v-if="type === 'Phone'" label="时间" type="number" placeholder="请输入电话咨询时间（分钟）" v-model="serviceDuration"></mt-field>
                <mt-field label="次数" placeholder="请输入日接单次数" type="number" v-model="serviceFrequency"></mt-field>
                <mt-radio
                title="是否开启"
                v-model="value"
                :options="options">
                </mt-radio>
                <div v-if="set != 'add'" @click="submitServer" class="set-ok">
                    <mt-button type="primary" class="mint-button--large">保存</mt-button>
                </div>
                <div v-if="set === 'add'" @click="addServer" class="set-ok">
                    <mt-button type="primary" class="mint-button--large">创建</mt-button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { Toast } from 'mint-ui';
export default {
    data () {
        return {
            show: true,
            id: 0,
            set: '',
            title: '',
            serviceExpense: '',
            serviceFrequency: '',
            type: '',
            serviceDuration: '',
            value:'1',
            options: [
                {   
                    label: '启用',
                    value: '1'
                },
                {
                    label: '关闭',
                    value: '0'
                }
            ]
        }
    },
    methods: {
        getUserServer() { // 获取我的服务
            const id =this.id
            this.instance.serviceMoneyListByIDD({
                id
            })
                .then((response) => {
                    const userServerList = response.data.result.item
                    this.serviceExpense = userServerList.serviceExpense
                    this.serviceFrequency = userServerList.serviceFrequency
                    this.serviceDuration = userServerList.serviceDuration
                    this.serviceState = userServerList.serviceState
                    if (this.serviceState) {
                        this.value = '1'
                    } else {
                        this.value = '0'
                    }
                })
                .catch((error) => {
                }) 
        },
        submitServer() { // 修改服务
            const id =this.id
            let serviceState = parseInt(this.value)
            let serviceExpense = parseFloat(this.serviceExpense)
            let serviceFrequency = parseFloat(this.serviceFrequency)
            let serviceDuration = this.serviceDuration
            if (!serviceDuration) this.serviceDuration = 0
            serviceDuration = parseInt(this.serviceDuration)

            let nubTest = /^[1-9]\d*$/
            
            let flag = nubTest.test(serviceExpense)
            let flag2 = nubTest.test(serviceFrequency)
            console.log(serviceExpense)
            console.log(flag)
            
            if (serviceExpense <= 0 || !flag) {
                Toast('价格为不小于0的整数')
                return
            }

            if (serviceFrequency <= 0 && !flag2) {
                Toast('次数为不小于0的整数')
                return
            }

            serviceExpense = parseInt(this.serviceExpense)
            serviceFrequency = parseInt(this.serviceFrequency)
            this.instance.updateServiceMoneyListD({
                id,
                serviceState,
                serviceExpense,
                serviceFrequency,
                serviceDuration
            })
                .then((response) => {
                    if (response.data.result.code === 200) {
                        Toast('修改成功')
                        this.$router.push({ 
                            path: '/server'
                        })
                    }
                })
                .catch((error) => {
                }) 
        },
        addServer() { // 创建服务
            let serviceState = parseInt(this.value)
            const serviceType = this.code
            const serviceExpense = parseInt(this.serviceExpense)
            const serviceFrequency = parseInt(this.serviceFrequency)
            let serviceDuration = this.serviceDuration
            if (!serviceDuration) this.serviceDuration = 0
            serviceDuration = parseInt(this.serviceDuration)
            let nubTest = /^[-]?\d+$/
            let flag = nubTest.test(serviceExpense)
            let flag2 = nubTest.test(serviceFrequency)
            if (serviceExpense <= 0 || !flag) {
                Toast('价格为不小于0的整数')
                return
            }

            if (serviceFrequency <= 0 && !flag2) {
                Toast('次数为不小于0的整数')
                return
            }

            this.instance.createServiceMoneyListD({
                serviceState,
                serviceType,
                serviceExpense,
                serviceFrequency,
                serviceDuration
            })
                .then((response) => {
                    if (response.data.result.code === 200) {
                        Toast('创建成功')
                        this.$router.push({ 
                            path: '/server'
                        })
                    }
                    
                })
                .catch((error) => {
                }) 
        }
    },
    mounted () {
        
    },
    created () {
    },
    activated () {
        this.set = this.$route.query.set
        if (this.set === 'add') {
            this.code = this.$route.query.code
            this.type = this.code
            this.title = '创建服务'
        } else {
            this.id = parseInt(this.$route.query.id)
            this.type = this.$route.query.type
            this.title = '设置服务'
            this.getUserServer()
        }
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.set-server {
    .set-type {background: $color-wfont; line-height: 40px; padding-left: 10px;}
    .set-ok {margin: 20px 10px;}
}

</style>