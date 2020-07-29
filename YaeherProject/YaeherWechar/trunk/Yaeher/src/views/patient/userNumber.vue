<template>
    <div class="user-number padding-top">
        <mt-header fixed title="我的成员">
            <a @click="$router.push({path: '/patient-info'})" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
            <mt-button  slot="right">
                <router-link to="/add-number" class="right-white">添加</router-link>
            </mt-button>           
        </mt-header>
        <div class="content">
            <ul class="user-list">
                <li v-for="(item, index) in numberList" :key="index">
                    <router-link :to="{path: '/number-detail', query: {id: item.id}}" class="user-href">
                        <div class="flex">
                            <div class="user-title user-title1">成员名：{{item.leaguerName}}</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
            </ul>    
        </div>
    </div>
</template>

<script>
import { Toast } from 'mint-ui';
export default {
    data () {
        return {
            numberList: []
        }
    },
    methods: {
        getConsultList() { // 请求个人信息
            this.instance.leaguerInfoList({
            })
                .then((response) => {
                    this.numberList = response.data.result.item
                })
                .catch((error) => {
                }) 
        }
    },
    mounted () {
        this.getConsultList()
    },
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.user-number {
    .numberOk {padding-top: 15px;}
}

</style>