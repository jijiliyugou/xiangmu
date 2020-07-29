<template>
    <div class="set-server padding-top">
        <mt-header fixed title="系统设置">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
            <div class="system-set">
                <mt-field :label="doctorParaSetName" placeholder="次数上限" v-model="itemValue"></mt-field>
                <div class="set-ok">
                    <mt-button @click="submitSys" type="primary" class="mint-button--large">保存</mt-button>
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
            id: 0,
            doctorParaSetName: '',
            itemValue: ''
        }
    },
    methods: {
        getSystemDetail () {
            const id = this.id
            this.instance.doctorParaSetByIdA({
                id
            })
                .then((response) => {
                    let sysDetail = response.data.result.item
                    this.doctorParaSetName = sysDetail.doctorParaSetName
                    this.itemValue = sysDetail.itemValue
                })
                .catch((error) => {

                }) 
        },
        submitSys() { // 提交参数修改
            const id = this.id
            const itemValue = this.itemValue
            const doctorParaSetName = this.doctorParaSetName
            if(!itemValue) {
                Toast('修改内容不能为空！')
                return
            }
            this.instance.updateDoctorParaSetA({
                id,
                itemValue,
                doctorParaSetName
            })
                .then((response) => {
                    if(response.data.result.code === 200) {
                        Toast('修改成功')
                        this.$router.go(-1)
                    }
                    
                })
                .catch((error) => {

                }) 
        }
    },
    mounted () {
        this.getSystemDetail()
    },
    created () {
        this.id = this.$route.query.id
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.set-server {
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