<template>
    <div class="system-setup padding-top">
        <mt-header fixed title="系统设置">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
            <div class="system-set">
                <ul class="sysList">
                    <router-link tag="li" :to="{path: 'system-detail', query: {id: item.id}}" v-for="(item, index) in sysDetail" :key="index">
                        <div>{{item.doctorParaSetName}}：</div>
                        <div>
                            {{item.itemValue}}
                            <i class="mint-cell-allow-right"></i>
                        </div>
                    </router-link>
                </ul>
                <!-- <mt-field label="图文咨询次数上限" placeholder="次数上限" v-model="maxTime"></mt-field>
                <mt-field label="追问次数" placeholder="请输入可追问次数" v-model="time"></mt-field>
                <mt-field label="字数长度" placeholder="请输入可输入字数长度" v-model="sizeLength"></mt-field>
                <mt-field label="退单限制时间" placeholder="请输入退单限制时间" v-model="chargeTime"></mt-field>
                <mt-field label="超时时间设置" placeholder="请输入超时时间" v-model="overtime"></mt-field> -->
                <!-- <div class="set-ok">
                    <mt-button @click="submitSys" type="primary" class="mint-button--large">保存</mt-button>
                </div> -->
            </div>
        </div>
    </div>
</template>

<script>
export default {
    data () {
        return {
            sysDetail: []
        }
    },
    methods: {
        getSystemDetail () {
            this.instance.doctorParaSetListA({
            })
                .then((response) => {
                    this.sysDetail = response.data.result.item
                    console.log(this.sysDetail)
                })
                .catch((error) => {

                }) 
        }
    },
    mounted () {
        this.getSystemDetail()
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.system-setup {
    .system-set {
        .mint-cell-title {
            width: 180px;
        }
        .sysList {
            li {
                display: flex;
                div {
                    flex: 1;
                    position: relative;
                    .mint-cell-allow-right::after {
                        right: 0px;
                    }
                }
            }
        }
    }
    .set-ok {margin: 20px 10px;}
}

</style>