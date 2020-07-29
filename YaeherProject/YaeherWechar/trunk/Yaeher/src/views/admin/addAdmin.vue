<template>
    <div class="add-admin padding-top">
        <mt-header fixed title="添加权限人员">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
            <div class="arrange-case">

                <mt-radio
                title="患者端权限"
                v-model="value"
                :options="options">
                </mt-radio>

                <mt-radio
                title="医生端权限"
                v-model="value1"
                :options="options1">
                </mt-radio>

                <mt-radio
                title="质控端权限"
                v-model="value2"
                :options="options2">
                </mt-radio>

                <mt-radio
                title="客服端权限"
                v-model="value3"
                :options="options3">
                </mt-radio>

                <mt-radio
                title="管理端权限"
                v-model="value4"
                :options="options4">
                </mt-radio>
                
                <div class="set-ok">
                    <router-link to="/doctor-info-list" class="okBtn">
                        <mt-button type="primary" class="mint-button--large">保存</mt-button>
                    </router-link>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    data () {
        return {
            show: true,
            price: '',
            address: '',
            value: '1',
            value1: '1',
            value2: '0',
            value3: '0',
            value4: '0',
            options: [
                {
                    label: '开启',
                    value: '1'
                },
                {
                    label: '关闭',
                    value: '0'
                }
            ],
            options1: [
                {   
                    label: '开启',
                    value: '1'
                },
                {
                    label: '关闭',
                    value: '0'
                }
            ],
            options2: [
                {   
                    label: '开启',
                    value: '1'
                },
                {
                    label: '关闭',
                    value: '0'
                }
            ],
            options3: [
                {   
                    label: '开启',
                    value: '1'
                },
                {
                    label: '关闭',
                    value: '0'
                }
            ],
            options4: [
                {   
                    label: '开启',
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
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.add-admin {
    .arrange-case{
        .mint-radiolist-title {
            font-size: $font-l;
        }
        .mint-checklist {
            .mint-cell {
                display: inline-block;
                width: 33.3333%;
                background-image: none;
                .mint-cell-wrapper {
                    background-image: none;
                    .mint-checkbox-core::after {
                        top: 4px;
                        left: 7px;
                    }
                }
            }
        }
        .mint-radiolist {
            .mint-cell {
                display: inline-block;
                width: 50%;
                background-image: none;
                .mint-cell-wrapper {
                    background-image: none;
                }
            }
        }
        .line-p {
            height: 5px;
        }
        .set-ok {
            margin: 20px 10px;
        }
    }
}





</style>